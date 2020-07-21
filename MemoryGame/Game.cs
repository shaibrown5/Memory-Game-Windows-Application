using System;
using System.Drawing;
using Players;
using GameDeck;

namespace MemoryGame
{
    public class Game
    {
        private readonly Player r_Player1;
        private readonly Player r_Player2H;
        private readonly ComputerPlayer r_Player2C;
        private readonly Deck r_Deck;
        private readonly bool r_IsHuman = true;
        private int m_PlayerTurn = 1;

        /**
         * Getter for first player
         */
        public Player PlayerOne
        {
            get
            {
                return r_Player1;
            }
        }

        /**
         * Getter for the computer player
         */
        public ComputerPlayer ComputerPlayer
        {
            get
            {
                return r_Player2C;
            }
        }

        /**
         * This methods returns the name of the second player
         */
        public string GetPlayerTwoName()
        {
            string name = r_IsHuman ? r_Player2H.Name : r_Player2C.Name;
            return name;
        }

        /**
         * This method gets the score of the second player
         */
        public int GetPlayerTwoScore()
        {
            int player2Score = r_IsHuman ? r_Player2H.Score : r_Player2C.Score;
            return player2Score;
        }

        /**
         * Getter for player turn
         */
        public int PlayerTurn
        {
            get
            {
                return m_PlayerTurn;
            }
        }

        /**
         * Constructor for a new game
         */
        public Game(string i_FirstPlayerName, string i_SecondPlayerName, bool i_IsHuman, Deck i_GameDeck)
        {
            //initiating stages of the game
            r_Deck = i_GameDeck;
            r_Player1 = new Player(i_FirstPlayerName, Color.FromArgb(((int) (((byte) (192)))), ((int) (((byte) (255)))), ((int) (((byte) (192))))));
            r_IsHuman = i_IsHuman;

            if (r_IsHuman)
            {
                r_Player2H = new Player(i_SecondPlayerName, Color.MediumPurple);
            }
            else
            {
                r_Player2C = new ComputerPlayer();
            }
        }

        /**
        * This method help for quick rebuilding the board after the pick.
        */
        public bool IsMatch(int i_FirstCardIndexInDeck, int i_SecondCardIndexInDeck)
        {
            bool hasMached = (r_Deck.GetCard(i_FirstCardIndexInDeck).Data.Equals(r_Deck.GetCard(i_SecondCardIndexInDeck).Data));

            if (hasMached)
            {
                if (PlayerTurn == 1)
                {
                    PlayerOne.Score++;
                }
                else if (r_IsHuman)
                {
                    r_Player2H.Score++;
                }
                else
                {
                    r_Player2C.Score++;
                }
            }
            else
            {
                m_PlayerTurn = m_PlayerTurn == 1 ? 2 : 1;
            }

            return hasMached;
        }

        /**
        * This method determines if the game is over.
        */
        public bool IsGameOver(int i_Col, int i_Row)
        {
            int pointSum = r_IsHuman ? r_Player1.Score + r_Player2H.Score : r_Player1.Score + r_Player2C.Score;

            return (i_Col * i_Row) / 2 == pointSum;
        }

        /**
         * returns the name of the winner
         */
        public string[] GetEndGameDetails(int i_Col, int i_Row)
        {
            // 0 = winner name, 1 = loser name, 2 = winning score, 3 = losing score
            string[] gameDetails = new string[4];
            int loosingScore = (i_Row * i_Col) / 2;

            if (r_IsHuman)
            {

                gameDetails[0] = r_Player1.Score > r_Player2H.Score ? r_Player1.Name : r_Player2H.Name;
                gameDetails[1] = r_Player1.Score > r_Player2H.Score ? r_Player2H.Name : r_Player1.Name;
                gameDetails[2] = r_Player1.Score > r_Player2H.Score
                    ? r_Player1.Score.ToString()
                    : r_Player2H.Score.ToString();
            }
            else
            {
                gameDetails[0] = r_Player1.Score > r_Player2C.Score ? r_Player1.Name : r_Player2C.Name;
                gameDetails[1] = r_Player1.Score > r_Player2C.Score ? r_Player2C.Name : r_Player1.Name;
                gameDetails[2] = r_Player1.Score > r_Player2C.Score
                    ? r_Player1.Score.ToString()
                    : r_Player2C.Score.ToString();
            }

            gameDetails[3] = (loosingScore - int.Parse(gameDetails[2])).ToString();

            return gameDetails;
        }
    }
}
