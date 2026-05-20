#!/usr/bin/env python3
"""
Unit tests for TrainerFeatures.
"""

import unittest
from unittest.mock import MagicMock, patch
from features import TrainerFeatures

class TestTrainerFeatures(unittest.TestCase):
    """Test suite for TrainerFeatures."""
    
    def setUp(self):
        """Set up test fixtures."""
        self.mock_mem = MagicMock()
        self.features = TrainerFeatures(self.mock_mem)
    
    def test_toggle_infinite_health_on(self):
        """Test turning infinite health on."""
        self.mock_mem.get_health.return_value = 100.0
        self.features.toggle_infinite_health()
        self.assertTrue(self.features.infinite_health_active)
        self.mock_mem.set_health.assert_called_with(100.0)
    
    def test_toggle_infinite_health_off(self):
        """Test turning infinite health off."""
        self.features.infinite_health_active = True
        self.features.original_health = 100.0
        self.features.toggle_infinite_health()
        self.assertFalse(self.features.infinite_health_active)
    
    def test_toggle_infinite_ammo_on(self):
        """Test turning infinite ammo on."""
        self.mock_mem.get_ammo.return_value = 10
        self.features.toggle_infinite_ammo()
        self.assertTrue(self.features.infinite_ammo_active)
        self.mock_mem.set_ammo.assert_called_with(999)
    
    def test_toggle_infinite_ammo_off(self):
        """Test turning infinite ammo off."""
        self.features.infinite_ammo_active = True
        self.features.original_ammo = 10
        self.mock_mem.get_ammo.return_value = 10
        self.features.toggle_infinite_ammo()
        self.assertFalse(self.features.infinite_ammo_active)
        self.mock_mem.set_ammo.assert_called_with(10)
    
    def test_add_health_normal(self):
        """Test adding health within cap."""
        self.mock_mem.get_health.return_value = 50.0
        self.features.add_health(100)
        self.mock_mem.set_health.assert_called_with(150.0)
    
    def test_add_health_cap(self):
        """Test adding health beyond cap."""
        self.mock_mem.get_health.return_value = 150.0
        self.features.add_health(100)
        self.mock_mem.set_health.assert_called_with(200.0)
    
    def test_add_ammo(self):
        """Test adding ammo."""
        self.mock_mem.get_ammo.return_value = 10
        self.features.add_ammo(50)
        self.mock_mem.set_ammo.assert_called_with(60)
    
    def test_save_position(self):
        """Test saving position."""
        self.mock_mem.get_position.return_value = (1.0, 2.0, 3.0)
        self.features.save_position()
        self.assertEqual(self.features.saved_position, (1.0, 2.0, 3.0))
    
    def test_teleport_with_saved_position(self):
        """Test teleporting to saved position."""
        self.features.saved_position = (10.0, 20.0, 30.0)
        self.features.teleport_to_saved_position()
        self.mock_mem.set_position.assert_called_with(10.0, 20.0, 30.0)
    
    def test_teleport_without_saved_position(self):
        """Test teleporting without a saved position."""
        self.features.saved_position = None
        with self.assertLogs() as log:
            self.features.teleport_to_saved_position()
            self.assertIn("No saved position", log.output[0])
    
    def test_apply_continuous_effects_infinite_health(self):
        """Test continuous health effect."""
        self.features.infinite_health_active = True
        self.features.original_health = 100.0
        self.features.apply_continuous_effects()
        self.mock_mem.set_health.assert_called_with(100.0)
    
    def test_apply_continuous_effects_infinite_ammo(self):
        """Test continuous ammo effect."""
        self.features.infinite_ammo_active = True
        self.features.apply_continuous_effects()
        self.mock_mem.set_ammo.assert_called_with(999)

if __name__ == '__main__':
    unittest.main()
