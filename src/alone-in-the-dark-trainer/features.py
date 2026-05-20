#!/usr/bin/env python3
"""
Trainer features for Alone in the Dark.
Provides toggleable cheats and utilities.
"""

class TrainerFeatures:
    """Encapsulates all cheat features."""
    
    def __init__(self, memory_manager):
        self.mem = memory_manager
        self.infinite_health_active = False
        self.infinite_ammo_active = False
        self.saved_position = None
        self.original_health = 100.0
        self.original_ammo = 10
    
    def toggle_infinite_health(self):
        """Toggle infinite health on/off."""
        self.infinite_health_active = not self.infinite_health_active
        if self.infinite_health_active:
            self.original_health = self.mem.get_health()
            print("Infinite Health: ON")
            # Continuously set health to original value
            self.mem.set_health(self.original_health)
        else:
            print("Infinite Health: OFF")
    
    def toggle_infinite_ammo(self):
        """Toggle infinite ammo on/off."""
        self.infinite_ammo_active = not self.infinite_ammo_active
        if self.infinite_ammo_active:
            self.original_ammo = self.mem.get_ammo()
            print("Infinite Ammo: ON")
            self.mem.set_ammo(999)
        else:
            print("Infinite Ammo: OFF")
            self.mem.set_ammo(self.original_ammo)
    
    def add_health(self, amount):
        """Add a specific amount of health."""
        current = self.mem.get_health()
        new_health = min(current + amount, 200.0)  # Cap at 200
        self.mem.set_health(new_health)
        print(f"Health increased by {amount} to {new_health}")
    
    def add_ammo(self, amount):
        """Add a specific amount of ammo."""
        current = self.mem.get_ammo()
        new_ammo = current + amount
        self.mem.set_ammo(new_ammo)
        print(f"Ammo increased by {amount} to {new_ammo}")
    
    def save_position(self):
        """Save current player position."""
        self.saved_position = self.mem.get_position()
        print(f"Position saved: {self.saved_position}")
    
    def teleport_to_saved_position(self):
        """Teleport to saved position."""
        if self.saved_position is None:
            print("No saved position to teleport to. Press F5 first.")
            return
        x, y, z = self.saved_position
        self.mem.set_position(x, y, z)
        print(f"Teleported to saved position: ({x:.2f}, {y:.2f}, {z:.2f})")
    
    def apply_continuous_effects(self):
        """Apply effects that need to run continuously (called in main loop)."""
        if self.infinite_health_active:
            self.mem.set_health(self.original_health)
        if self.infinite_ammo_active:
            self.mem.set_ammo(999)
