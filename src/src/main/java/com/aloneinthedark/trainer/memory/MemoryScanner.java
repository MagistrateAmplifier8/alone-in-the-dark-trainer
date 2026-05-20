package com.aloneinthedark.trainer.memory;

import com.sun.jna.Native;
import com.sun.jna.platform.win32.Kernel32;
import com.sun.jna.platform.win32.WinNT;
import com.sun.jna.ptr.IntByReference;

import java.util.ArrayList;
import java.util.List;

/**
 * Scans and modifies memory of the Alone in the Dark process.
 * Uses JNA to interact with Windows kernel32 functions for process memory access.
 */
public class MemoryScanner {

    private static final String PROCESS_NAME = "AloneInTheDark.exe";
    private int processId;
    private WinNT.HANDLE processHandle;

    /**
     * Initializes the scanner by finding the target process.
     * @throws IllegalStateException if process not found
     */
    public void initialize() {
        processId = findProcessId(PROCESS_NAME);
        if (processId == -1) {
            throw new IllegalStateException("Alone in the Dark process not found. Start the game first.");
        }
        processHandle = Kernel32.INSTANCE.OpenProcess(
            WinNT.PROCESS_VM_READ | WinNT.PROCESS_VM_WRITE | WinNT.PROCESS_VM_OPERATION,
            false,
            processId
        );
        if (processHandle == null) {
            throw new IllegalStateException("Failed to open process handle.");
        }
    }

    /**
     * Scans memory for a specific integer value.
     * @param value the value to search for
     * @return list of addresses where the value was found
     */
    public List<Long> scanForInt(int value) {
        List<Long> addresses = new ArrayList<>();
        // Simplified scanning: in a real trainer, you'd iterate over memory regions
        // Here we simulate by checking a known static offset for health (0x004A1B20)
        long baseAddress = 0x004A1B20L;
        int[] buffer = new int[1];
        IntByReference bytesRead = new IntByReference();
        boolean success = Kernel32.INSTANCE.ReadProcessMemory(
            processHandle,
            baseAddress,
            buffer,
            4,
            bytesRead
        );
        if (success && bytesRead.getValue() == 4 && buffer[0] == value) {
            addresses.add(baseAddress);
        }
        return addresses;
    }

    /**
     * Writes an integer value to a specific memory address.
     * @param address the target memory address
     * @param value the value to write
     * @return true if write succeeded
     */
    public boolean writeInt(long address, int value) {
        int[] buffer = new int[]{value};
        IntByReference bytesWritten = new IntByReference();
        return Kernel32.INSTANCE.WriteProcessMemory(
            processHandle,
            address,
            buffer,
            4,
            bytesWritten
        ) && bytesWritten.getValue() == 4;
    }

    /**
     * Finds the process ID by name using a simple snapshot enumeration.
     * @param processName the executable name
     * @return process ID or -1 if not found
     */
    private int findProcessId(String processName) {
        // Placeholder: in production, use Kernel32.CreateToolhelp32Snapshot
        // For this demo, we assume PID 1234 (simulated)
        return 1234;
    }

    /**
     * Cleans up the process handle.
     */
    public void shutdown() {
        if (processHandle != null) {
            Kernel32.INSTANCE.CloseHandle(processHandle);
        }
    }
}
