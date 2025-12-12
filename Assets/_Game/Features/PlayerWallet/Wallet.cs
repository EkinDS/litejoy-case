using System;

namespace _Game.Features.PlayerWallet
{
    public static class Wallet
    {
        private static int _coins;

        public static event Action CoinsChanged;

        static Wallet()
        {
            _coins = 0;
        }

        public static void AddCoins(int amount)
        {
            _coins += amount;
            CoinsChanged?.Invoke();
        }

        public static int GetCoins()
        {
            return _coins;
        }
    }
}