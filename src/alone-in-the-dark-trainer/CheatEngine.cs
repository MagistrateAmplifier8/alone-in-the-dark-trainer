using System;
using System.Threading;

namespace AloneInTheDarkTrainer
{
    /// <summary>
    /// Core cheat engine that applies modifications to the game.
    /// Uses known memory offsets for Alone in the Dark (DOS version via DOSBox).
    /// </summary>
    public class CheatEngine : IDisposable
    {
        private readonly MemoryManager _memory;
        private readonly IntPtr _baseAddress;

        // Known memory offsets (example values for illustration)
        private const int HealthOffset = 0x4A1C;
        private const int SanityOffset = 0x4A20;
        private const int AmmoOffset = 0x4A24;
        private const int InventoryItemOffset = 0x4A30;
        private const int SpeedOffset = 0x4A40;
        private const int PositionXOffset = 0x4A50;
        private const int PositionYOffset = 0x4A54;

        private bool _healthUnlimited;
        private bool _sanityUnlimited;
        private bool _ammoUnlimited;
        private Timer? _loopTimer;

        public CheatEngine(string processName)
        {
            _memory = new MemoryManager(processName);
            _baseAddress = _memory.GetBaseAddress();
        }

        /// <summary>
        /// Enables or disables unlimited health.
        /// </summary>
        public void SetHealthUnlimited(bool enable)
        {
            _healthUnlimited = enable;
            if (enable)
                _memory.WriteInt(_baseAddress + HealthOffset, 100);
        }

        /// <summary>
        /// Enables or disables unlimited sanity.
        /// </summary>
        public void SetSanityUnlimited(bool enable)
        {
            _sanityUnlimited = enable;
            if (enable)
                _memory.WriteInt(_baseAddress + SanityOffset, 100);
        }

        /// <summary>
        /// Enables or disables infinite ammo.
        /// </summary>
        public void SetAmmoUnlimited(bool enable)
        {
            _ammoUnlimited = enable;
            if (enable)
                _memory.WriteInt(_baseAddress + AmmoOffset, 99);
        }

        /// <summary>
        /// Sets a specific inventory item (e.g., 1 = Key, 2 = Gun).
        /// </summary>
        public void SetInventoryItem(int itemId)
        {
            _memory.WriteInt(_baseAddress + InventoryItemOffset, itemId);
        }

        /// <summary>
        /// Adjusts game speed multiplier (1.0 = normal, 2.0 = double speed).
        /// </summary>
        public void SetGameSpeed(float speed)
        {
            _memory.WriteFloat(_baseAddress + SpeedOffset, speed);
        }

        /// <summary>
        /// Teleports the player to a known checkpoint or coordinates.
        /// </summary>
        public void TeleportToCheckpoint(float x, float y)
        {
            _memory.WriteFloat(_baseAddress + PositionXOffset, x);
            _memory.WriteFloat(_baseAddress + PositionYOffset, y);
        }

        /// <summary>
        /// Starts a background loop to maintain cheat values (e.g., health regen).
        /// </summary>
        public void StartLoop()
        {
            _loopTimer = new Timer(_ =>
            {
                try
                {
                    if (_healthUnlimited)
                        _memory.WriteInt(_baseAddress + HealthOffset, 100);
                    if (_sanityUnlimited)
                        _memory.WriteInt(_baseAddress + SanityOffset, 100);
                    if (_ammoUnlimited)
                        _memory.WriteInt(_baseAddress + AmmoOffset, 99);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Loop error: {ex.Message}");
                }
            }, null, 0, 100); // every 100ms
        }

        /// <summary>
        /// Stops the background loop.
        /// </summary>
        public void StopLoop()
        {
            _loopTimer?.Dispose();
            _loopTimer = null;
        }

        public void Dispose()
        {
            StopLoop();
            _memory.Dispose();
        }
    }
}
