namespace __Scripts
{
    public static class Utils
    {
        //Extension methods for general utility more of a learning than utility;
        public static bool IsTurn(this GameStateData gameData, string player) 
        {
            return gameData.playerTurn.ToString() == player;
        }

        public static void UpdatePieces(this GameStateData gameStateData, string player, int amount)
        {
            if (player == "Red")
            {
                gameStateData.numRedPieces += amount;
            }
            else gameStateData.numBluePieces += amount;
        }

        public static string GetOppositeColour(this GameStateData gameStateData, string playerColour)
        {
            return playerColour == "Blue" ? "Red" : "Blue";
        }
    }
}