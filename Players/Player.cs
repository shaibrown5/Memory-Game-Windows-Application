using System.Drawing;

namespace Players
{
    public class Player
    {
        // Fields
        private readonly string r_Name;
        private int m_Score = 0;
        private readonly Color r_PlayerColor;
        
        /**
         * Getter for the player name
         */
        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        /**
         * Getter and setter for the players score
         */
        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        /**
         * Getter for the player's color
         */
        public Color PlayerColor
        {
            get
            {
                return r_PlayerColor;
            }
        }

        /*
         * The Player constructor
         * init the Player Name by the input i_Name.
         */
        public Player(string i_Name, Color i_PlayerColor)
        {
            this.r_Name = i_Name;
            this.r_PlayerColor = i_PlayerColor;
        }
    }
}
