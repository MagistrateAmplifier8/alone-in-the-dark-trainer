#!/usr/bin/env python3
"""
Memory manager for interacting with the Alone in the Dark process.
Uses pymem to read/write process memory.
"""

import pymem

class MemoryManager:
    """Handles process attachment and memory operations."""
    
    def __init__(self):
        self.process_name = "Alone.exe"
        self.pm = None
        self.base_address = None
        
        # Offsets for key values (example addresses, not real)
        self.health_offset = 0x00A3F4C0
        self.ammo_offset = 0x00A3F4C8
        self.position_x_offset = 0x00A3F4D0
        self.position_y_offset = 0x00A3F4D4
        self.position_z_offset = 0x00A3F4D8
        
    def attach(self):
        """Attach to the game process."""
        try:
            self.pm = pymem.Pymem(self.process_name)
            self.base_address = self.pm.process_base.lpBaseOfDll
            print(f"Attached to {self.process_name} at base 0x{self.base_address:x}")
        except pymem.exception.ProcessNotFound:
            raise RuntimeError(f"Process {self.process_name} not found. Is the game running?")
    
    def detach(self):
        """Detach from the process."""
        if self.pm:
            self.pm.close_process()
            print("Detached from process.")
    
    def read_float(self, offset):
        """Read a float value from a given offset."""
        address = self.base_address + offset
        return self.pm.read_float(address)
    
    def write_float(self, offset, value):
        """Write a float value to a given offset."""
        address = self.base_address + offset
        self.pm.write_float(address, value)
    
    def read_int(self, offset):
        """Read an integer value from a given offset."""
        address = self.base_address + offset
        return self.pm.read_int(address)
    
    def write_int(self, offset, value):
        """Write an integer value to a given offset."""
        address = self.base_address + offset
        self.pm.write_int(address, value)
    
    def get_health(self):
        """Get current health."""
        return self.read_float(self.health_offset)
    
    def set_health(self, value):
        """Set health to a specific value."""
        self.write_float(self.health_offset, value)
    
    def get_ammo(self):
        """Get current ammo count."""
        return self.read_int(self.ammo_offset)
    
    def set_ammo(self, value):
        """Set ammo count."""
        self.write_int(self.ammo_offset, value)
    
    def get_position(self):
        """Get position as (x, y, z)."""
        x = self.read_float(self.position_x_offset)
        y = self.read_float(self.position_y_offset)
        z = self.read_float(self.position_z_offset)
        return (x, y, z)
    
    def set_position(self, x, y, z):
        """Set position to (x, y, z)."""
        self.write_float(self.position_x_offset, x)
        self.write_float(self.position_y_offset, y)
        self.write_float(self.position_z_offset, z)
