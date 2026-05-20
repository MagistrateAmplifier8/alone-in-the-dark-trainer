using NUnit.Framework;
using System;

namespace AloneInTheDarkTrainer.Tests
{
    [TestFixture]
    public class CheatEngineTests
    {
        private CheatEngine? _engine;

        [SetUp]
        public void Setup()
        {
            // Note: These tests require the game process to be running.
            // If not, they will throw. For unit testing without game,
            // we mock the MemoryManager. Here we test logic only.
            try
            {
                _engine = new CheatEngine("ALONE");
            }
            catch (InvalidOperationException)
            {
                // Game not running, skip tests that require it.
                _engine = null;
            }
        }

        [TearDown]
        public void Teardown()
        {
            _engine?.Dispose();
        }

        [Test]
        public void SetHealthUnlimited_DoesNotThrow()
        {
            if (_engine == null)
                Assert.Ignore("Game process not found; skipping.");

            Assert.DoesNotThrow(() => _engine!.SetHealthUnlimited(true));
            Assert.DoesNotThrow(() => _engine!.SetHealthUnlimited(false));
        }

        [Test]
        public void SetSanityUnlimited_DoesNotThrow()
        {
            if (_engine == null)
                Assert.Ignore("Game process not found; skipping.");

            Assert.DoesNotThrow(() => _engine!.SetSanityUnlimited(true));
            Assert.DoesNotThrow(() => _engine!.SetSanityUnlimited(false));
        }

        [Test]
        public void SetAmmoUnlimited_DoesNotThrow()
        {
            if (_engine == null)
                Assert.Ignore("Game process not found; skipping.");

            Assert.DoesNotThrow(() => _engine!.SetAmmoUnlimited(true));
            Assert.DoesNotThrow(() => _engine!.SetAmmoUnlimited(false));
        }

        [Test]
        public void SetInventoryItem_WithValidId_DoesNotThrow()
        {
            if (_engine == null)
                Assert.Ignore("Game process not found; skipping.");

            Assert.DoesNotThrow(() => _engine!.SetInventoryItem(1));
            Assert.DoesNotThrow(() => _engine!.SetInventoryItem(2));
        }

        [Test]
        public void SetGameSpeed_NormalSpeed_DoesNotThrow()
        {
            if (_engine == null)
                Assert.Ignore("Game process not found; skipping.");

            Assert.DoesNotThrow(() => _engine!.SetGameSpeed(1.0f));
        }

        [Test]
        public void TeleportToCheckpoint_DoesNotThrow()
        {
            if (_engine == null)
                Assert.Ignore("Game process not found; skipping.");

            Assert.DoesNotThrow(() => _engine!.TeleportToCheckpoint(100.0f, 200.0f));
        }

        [Test]
        public void StartStopLoop_DoesNotThrow()
        {
            if (_engine == null)
                Assert.Ignore("Game process not found; skipping.");

            Assert.DoesNotThrow(() => _engine!.StartLoop());
            Assert.DoesNotThrow(() => _engine!.StopLoop());
        }
    }
}
