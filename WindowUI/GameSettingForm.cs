using System;
using System.Windows.Forms;

namespace WindowUI
{
    public partial class GameSettingForm : Form
    {
        private int m_BoardSizeClickIndex = 0;

        /**
         * Constructor fo the game setting form
         */
        public GameSettingForm()
        {
            InitializeComponent();
        }

        /**
         * getter for the first players name
         */
        public string FirstPlayerName
        {
            get
            {
                return textBoxFirstPlayer.Text;
            }
        }

        /**
         * getter for the second players name
         */
        public string SecondPlayerName
        {
            get
            {
                return textBoxSecondPlayer.Text;
            }
        }

        /**
         * getter for the chosen board size
         */
        public string BoardSize
        {
            get
            {
                return buttonBoardSize.Text;
            }
        }

        /**
         * This method enables the start button if the first players name is not empty
         */
        private void textBoxFirstPlayer_TextChanged(object sender, EventArgs e)
        {
            buttonStart.Enabled = true;

            if (String.IsNullOrWhiteSpace((sender as TextBox).Text) || String.IsNullOrWhiteSpace(textBoxSecondPlayer.Text))
            {
                buttonStart.Enabled = false;
            }
        }

        /**
         * This method enables the start button if the second player name is not empty
         */
        private void textBoxSecondPlayer_TextChanged(object sender, EventArgs e)
        {
            buttonStart.Enabled = true;

            if (String.IsNullOrWhiteSpace((sender as TextBox).Text) || String.IsNullOrWhiteSpace(textBoxFirstPlayer.Text))
            {
                buttonStart.Enabled = false;
            }
        }

        /**
         * This method handles the second player option
         * changes the second player to a human opponent
         */
        private void buttonAgainstFriend_Click(object sender, EventArgs e)
        {
            textBoxSecondPlayer.Enabled = true;
            textBoxSecondPlayer.Text = string.Empty;
            buttonAgainst.Text = "Against Computer";

            this.buttonAgainst.Click -= new System.EventHandler(this.buttonAgainstFriend_Click);
            this.buttonAgainst.Click += new System.EventHandler(this.buttonAgainstComputer_Click);
        }

        /**
         * This method handles the second player option
         * changes the second player back to computer
         */
        private void buttonAgainstComputer_Click(object sender, EventArgs e)
        {
            textBoxSecondPlayer.Enabled = false;
            textBoxSecondPlayer.Text = "- computer -";
            buttonAgainst.Text = "Against a Friend";

            this.buttonAgainst.Click -= new System.EventHandler(this.buttonAgainstComputer_Click);
            this.buttonAgainst.Click += new System.EventHandler(this.buttonAgainstFriend_Click);
        }

        /**
         * This method changes the board size according to the players choice
         */
        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            string[] boardSizes = {"4 x 4", "4 x 5", "4 x 6", "5 x 4", "5 x 6", "6 x 4", "6 x 5", "6 x 6"};
            buttonBoardSize.Text = boardSizes[++m_BoardSizeClickIndex % 8];

            //not to overflow int
            if (m_BoardSizeClickIndex % 8 == 0)
            {
                m_BoardSizeClickIndex = 0;
            }
        }

        /**
         * This method handles the start button events
         * Starts the game when used
         */
        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.Hide();
            bool isHuman = textBoxSecondPlayer.Enabled;
            MemoryGameForm memoryGameForm = new MemoryGameForm(FirstPlayerName, SecondPlayerName, BoardSize, isHuman);
            memoryGameForm.ShowDialog();
            this.Close();
        }
    }
}
