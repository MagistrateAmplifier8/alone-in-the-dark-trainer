#!/usr/bin/env python3
"""
Alone in the Dark Trainer - Main entry point.
Provides a console-based trainer for the classic game.
"""

import sys
import time
import keyboard
from memory import MemoryManager
from features import TrainerFeatures

def main():
    print("=== Alone in the Dark Trainer v1.0 ===")
    print("Attaching to game process...")
    
    try:
        mem = MemoryManager()
        mem.attach()
    except Exception as e:
        print(f"Failed to attach: {e}")
        sys.exit(1)
    
    features = TrainerFeatures(mem)
    print("Trainer active. Hotkeys:")
    print("  F1 - Toggle Infinite Health")
    print("  F2 - Toggle Infinite Ammo")
    print("  F3 - Add 100 Health")
    print("  F4 - Add 50 Ammo")
    print("  F5 - Save Position")
    print("  F6 - Teleport to Saved Position")
    print("  F7 - Exit")
    
    running = True
    while running:
        if keyboard.is_pressed('F1'):
            features.toggle_infinite_health()
            time.sleep(0.2)
        elif keyboard.is_pressed('F2'):
            features.toggle_infinite_ammo()
            time.sleep(0.2)
        elif keyboard.is_pressed('F3'):
            features.add_health(100)
            time.sleep(0.2)
        elif keyboard.is_pressed('F4'):
            features.add_ammo(50)
            time.sleep(0.2)
        elif keyboard.is_pressed('F5'):
            features.save_position()
            time.sleep(0.2)
        elif keyboard.is_pressed('F6'):
            features.teleport_to_saved_position()
            time.sleep(0.2)
        elif keyboard.is_pressed('F7'):
            print("Exiting...")
            running = False
        
        time.sleep(0.05)
    
    mem.detach()

if __name__ == "__main__":
    main()
