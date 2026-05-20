package com.aloneinthedark.trainer;

import com.aloneinthedark.trainer.ui.TrainerWindow;
import javax.swing.*;

/**
 * Entry point for the Alone in the Dark Trainer application.
 * Launches the Swing-based GUI for memory manipulation.
 */
public class Main {
    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            TrainerWindow window = new TrainerWindow();
            window.setVisible(true);
        });
    }
}
