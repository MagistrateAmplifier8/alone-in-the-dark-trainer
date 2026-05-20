package com.aloneinthedark.trainer.ui;

import com.aloneinthedark.trainer.features.TrainerFeatures;
import com.aloneinthedark.trainer.memory.MemoryScanner;

import javax.swing.*;
import java.awt.*;

/**
 * Swing-based GUI for the Alone in the Dark Trainer.
 * Provides buttons to activate cheats and display status.
 */
public class TrainerWindow extends JFrame {

    private final TrainerFeatures features;
    private final JLabel statusLabel;

    public TrainerWindow() {
        setTitle("Alone in the Dark Trainer v1.0");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(400, 300);
        setLayout(new FlowLayout());

        // Initialize memory scanner and features
        MemoryScanner scanner = new MemoryScanner();
        try {
            scanner.initialize();
            features = new TrainerFeatures(scanner);
        } catch (IllegalStateException e) {
            JOptionPane.showMessageDialog(this, e.getMessage(), "Error", JOptionPane.ERROR_MESSAGE);
            features = null;
            System.exit(1);
        }

        // Create UI components
        JButton healthButton = new JButton("Set Max Health");
        JButton ammoButton = new JButton("Set Max Ammo");
        JButton inventoryButton = new JButton("Add 5 Inventory Items");
        statusLabel = new JLabel("Ready");

        // Add action listeners
        healthButton.addActionListener(e -> {
            if (features.setMaxHealth()) {
                statusLabel.setText("Health set to 100!");
            } else {
                statusLabel.setText("Failed to set health.");
            }
        });

        ammoButton.addActionListener(e -> {
            if (features.setMaxAmmo()) {
                statusLabel.setText("Ammo set to 999!");
            } else {
                statusLabel.setText("Failed to set ammo.");
            }
        });

        inventoryButton.addActionListener(e -> {
            if (features.addInventoryItems(5)) {
                statusLabel.setText("5 items added to inventory!");
            } else {
                statusLabel.setText("Failed to add items.");
            }
        });

        // Layout
        add(healthButton);
        add(ammoButton);
        add(inventoryButton);
        add(statusLabel);

        // Cleanup on close
        addWindowListener(new java.awt.event.WindowAdapter() {
            @Override
            public void windowClosing(java.awt.event.WindowEvent e) {
                scanner.shutdown();
            }
        });
    }
}
