package com.aloneinthedark.trainer.features;

import com.aloneinthedark.trainer.memory.MemoryScanner;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/**
 * Unit tests for TrainerFeatures using a mock MemoryScanner.
 * Since real memory access requires the game, we mock the scanner.
 */
class TrainerFeaturesTest {

    private TrainerFeatures features;
    private MemoryScanner mockScanner;

    @BeforeEach
    void setUp() {
        // Create a simple mock that always succeeds
        mockScanner = new MemoryScanner() {
            @Override
            public boolean writeInt(long address, int value) {
                return true;
            }

            @Override
            public java.util.List<Long> scanForInt(int value) {
                return java.util.List.of(0x004A1B20L);
            }

            @Override
            public void initialize() {
                // no-op
            }

            @Override
            public void shutdown() {
                // no-op
            }
        };
        features = new TrainerFeatures(mockScanner);
    }

    @Test
    void testSetMaxHealthReturnsTrue() {
        assertTrue(features.setMaxHealth());
    }

    @Test
    void testSetMaxAmmoReturnsTrue() {
        assertTrue(features.setMaxAmmo());
    }

    @Test
    void testAddInventoryItemsReturnsTrue() {
        assertTrue(features.addInventoryItems(5));
    }

    @Test
    void testGetHealthReturnsPositive() {
        int health = features.getHealth();
        assertTrue(health > 0, "Health should be positive");
    }
}
