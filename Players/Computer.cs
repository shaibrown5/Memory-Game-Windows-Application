using System;
using System.Drawing;

namespace Players
{
    public class ComputerPlayer
    {
        // Fields
        private readonly string r_Name;
        private int m_Score = 0;
        private readonly Color r_CompColor = Color.MediumPurple;
        private readonly Random r_Random = new Random();

        /**
         * Getter for the computer names
         */
        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        /**
         * Getter and setter for the computer score
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
         * Getter for the computer color
         */
        public Color PlayerColor
        {
            get
            {
                return r_CompColor;
            }
        }

        /*
         * The ComputerPlayer constructor
         * init the name for the computer to be "computer player"
         */
        public ComputerPlayer()
        {
            this.r_Name = "computer";
        }

        /**
         * This method receives a rang of the buttons list and returns a random value in it it
         */
        public int Turn(int i_Range)
        {
            return r_Random.Next(0, i_Range);
        }
    }
}
