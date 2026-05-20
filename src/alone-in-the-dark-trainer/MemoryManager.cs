using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AloneInTheDarkTrainer
{
    /// <summary>
    /// Manages memory operations for the Alone in the Dark process.
    /// Provides read/write access to specific memory addresses.
    /// </summary>
    public class MemoryManager : IDisposable
    {
        private IntPtr _processHandle;
        private readonly int _processId;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        private const uint PROCESS_ALL_ACCESS = 0x1F0FFF;

        /// <summary>
        /// Initializes memory manager by finding the game process.
        /// </summary>
        /// <param name="processName">Name of the game process (e.g., "ALONE")</param>
        public MemoryManager(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
                throw new InvalidOperationException($"Process '{processName}' not found.");

            _processId = processes[0].Id;
            _processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, _processId);

            if (_processHandle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to open process handle.");
        }

        /// <summary>
        /// Reads a 4-byte integer from the specified memory address.
        /// </summary>
        public int ReadInt(IntPtr address)
        {
            byte[] buffer = new byte[4];
            if (!ReadProcessMemory(_processHandle, address, buffer, 4, out int bytesRead) || bytesRead != 4)
                throw new InvalidOperationException($"Failed to read memory at 0x{address.ToInt64():X}");

            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Writes a 4-byte integer to the specified memory address.
        /// </summary>
        public void WriteInt(IntPtr address, int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (!WriteProcessMemory(_processHandle, address, buffer, 4, out int bytesWritten) || bytesWritten != 4)
                throw new InvalidOperationException($"Failed to write memory at 0x{address.ToInt64():X}");
        }

        /// <summary>
        /// Reads a float from the specified memory address.
        /// </summary>
        public float ReadFloat(IntPtr address)
        {
            byte[] buffer = new byte[4];
            if (!ReadProcessMemory(_processHandle, address, buffer, 4, out int bytesRead) || bytesRead != 4)
                throw new InvalidOperationException($"Failed to read memory at 0x{address.ToInt64():X}");

            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Writes a float to the specified memory address.
        /// </summary>
        public void WriteFloat(IntPtr address, float value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (!WriteProcessMemory(_processHandle, address, buffer, 4, out int bytesWritten) || bytesWritten != 4)
                throw new InvalidOperationException($"Failed to write memory at 0x{address.ToInt64():X}");
        }

        /// <summary>
        /// Gets the base address of the main module.
        /// </summary>
        public IntPtr GetBaseAddress()
        {
            using var process = Process.GetProcessById(_processId);
            return process.MainModule?.BaseAddress ?? IntPtr.Zero;
        }

        public void Dispose()
        {
            if (_processHandle != IntPtr.Zero)
            {
                CloseHandle(_processHandle);
                _processHandle = IntPtr.Zero;
            }
        }
    }
}
