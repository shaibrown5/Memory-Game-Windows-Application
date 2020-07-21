using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GameDeck;
using MemoryGame;

namespace WindowUI
{
    public partial class MemoryGameForm : Form
    {
        private readonly int r_Row;
        private readonly int r_Col;
        private readonly string r_SecondPlayerName;
        private readonly string r_FirstPlayerName;
        private readonly bool r_IsHuman;
        private readonly Deck r_GameDeck;
        private readonly Game r_GameManager;
        private readonly List<Button> r_ClickedButtonList = new List<Button>();
        private readonly List<Button> r_UnmatchedButtonsList = new List<Button>();
        private bool m_InternetConnected = true;

        /**
         * Constructor for the memory game form
         */
        public MemoryGameForm(string i_FirstPlayerName, string i_SecondPlayerName, string i_BoardSize, bool i_IsHuman)
        {
            r_Col = int.Parse(i_BoardSize[0].ToString());
            r_Row = int.Parse(i_BoardSize[i_BoardSize.Length-1].ToString());
            r_GameDeck = new Deck(r_Row*r_Col);
            r_FirstPlayerName = i_FirstPlayerName;
            r_SecondPlayerName = i_IsHuman ? i_SecondPlayerName : "computer";
            r_IsHuman = i_IsHuman;
            r_GameManager = new Game(i_FirstPlayerName, i_SecondPlayerName, i_IsHuman, r_GameDeck);

            InitializeComponent();
            initializeForm();
        }

        /**
         * This method initializes the form according to the user specs
         */
        private void initializeForm()
        {
            //set the board size
            if (r_Col != 6)
            {
                adjustBoardSize("c");
            }

            if (r_Row != 6)
            {
                adjustBoardSize("r");
            }

            //init the labels
            updatePlayerScore();
            updateCurrentPlayer();
            initiateCards();
        }

        /**
         * This method assigns a card to each button
         */
        private void initiateCards()
        {
            int cardIndex = 0;
            
            foreach (Button button in Controls.OfType<Button>())
            {
                if(button.Enabled)
                {
                    button.Tag = cardIndex;
                    r_UnmatchedButtonsList.Add(button);
                    cardIndex++;
                }
            }

            if (r_GameDeck.GetCard((int)r_UnmatchedButtonsList[0].Tag).CardImage == null)
            {
                m_InternetConnected = false;
                noInternetGameFormat();
            }
        }

        /**
         * This method updates the name shown in the current player label
         */
        private void updateCurrentPlayer()
        {
            string currentPlayerName = r_GameManager.PlayerTurn == 1 ? r_FirstPlayerName : r_SecondPlayerName;
            labelCurrentPlayer.BackColor = r_GameManager.PlayerTurn == 1
                ? r_GameManager.PlayerOne.PlayerColor
                : Color.MediumPurple; 
            labelCurrentPlayer.Text = string.Format("Current Player: {0}", currentPlayerName);
            Application.DoEvents();
        }

        /**
         * This method receive either a c for column or r for row adjusts the board size accordingly
         */
        private void adjustBoardSize(string i_ColOrRow)
        {
            string buttonNameFilter = i_ColOrRow.Equals("c") ? ("6x") : ("x6");
            int numOfRowOrColToRemove = i_ColOrRow.Equals("c") ? 6 - r_Col : 6 - r_Row;

            while(numOfRowOrColToRemove > 0)
            {
                foreach (Button button in Controls.OfType<Button>())
                {
                    if (button.Name.Contains(buttonNameFilter))
                    {
                        button.Hide();
                        button.Enabled = false;
                    }
                }

                if (i_ColOrRow.Equals("c"))
                {
                    this.ClientSize = new System.Drawing.Size(this.ClientSize.Width - 100, this.ClientSize.Height);
                }
                else if (i_ColOrRow.Equals("r"))
                {
                    this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - 85);
                }

                buttonNameFilter = i_ColOrRow.Equals("c") ? ("5x") : ("x5");
                numOfRowOrColToRemove--;
            }
        }

        /**
         * This method deals with a button click event
         */
        private void button_Click(object sender, EventArgs e)
        {
            Button currentButton = (sender as Button);
            Color currentPlayerColor = r_GameManager.PlayerTurn == 1 ? r_GameManager.PlayerOne.PlayerColor : Color.MediumPurple;
            currentButton.BackgroundImage = r_GameDeck.GetCard((int)currentButton.Tag).CardImage;
            currentButton.FlatStyle = FlatStyle.Flat;
            currentButton.FlatAppearance.BorderColor = currentPlayerColor;
            currentButton.FlatAppearance.BorderSize = 4;
            currentButton.Enabled = false;

            r_ClickedButtonList.Add(currentButton);
            //if the player chose 2 cards
            if (r_ClickedButtonList.Count == 2)
            {
                endOfTurnChecks();
            }
        }

        /**
         * Button click event in the case that there is no internet
         */
        private void buttonNoInternet_Click(object sender, EventArgs e)
        {
            Button currentButton = (sender as Button);
            Color currentPlayerColor = r_GameManager.PlayerTurn == 1 ? r_GameManager.PlayerOne.PlayerColor : Color.MediumPurple;
            currentButton.BackColor = currentPlayerColor;
            currentButton.Text = r_GameDeck.GetCard((int)currentButton.Tag).Data;
            currentButton.Enabled = false;

            r_ClickedButtonList.Add(currentButton);
            //if the player chose 2 cards
            if (r_ClickedButtonList.Count == 2)
            {
                endOfTurnChecks();
            }
        }

        /**
         * this method handles all checks after the player chose 2 cards
         */
        private void endOfTurnChecks()
        {
            disableButtons();
            System.Threading.Thread.Sleep(1000);
            //if there was a match
            if (!r_GameManager.IsMatch((int)r_ClickedButtonList[0].Tag, (int)r_ClickedButtonList[1].Tag))
            {
                updateCurrentPlayer();
                cardTurnOver();
            }
            else
            {
                updatePlayerScore();
                r_UnmatchedButtonsList.Remove(r_ClickedButtonList[0]);
                r_UnmatchedButtonsList.Remove(r_ClickedButtonList[1]);
            }

            r_ClickedButtonList.Clear();
            enableButtons();
            //if the game is over
            if (r_GameManager.IsGameOver(r_Col, r_Row))
            {
                askGameReplay();
            }
            else if (!r_IsHuman && r_GameManager.PlayerTurn == 2)
            {
                computerMove();
            }
        } 

        /**
         * This method is in charge of the computer play
         */
        private void computerMove()
        {
            int firstCard = r_GameManager.ComputerPlayer.Turn(r_UnmatchedButtonsList.Count);
            Button firstButton = r_UnmatchedButtonsList[firstCard];
            int secondCard = r_GameManager.ComputerPlayer.Turn(r_UnmatchedButtonsList.Count);

            System.Threading.Thread.Sleep(500);
            firstButton.PerformClick();
            System.Threading.Thread.Sleep(1000);
            //makes sure that the cards are different
            while (secondCard == firstCard)
            {
                secondCard = r_GameManager.ComputerPlayer.Turn(r_UnmatchedButtonsList.Count);
            }

            Button secondButton = r_UnmatchedButtonsList[secondCard];
            secondButton.PerformClick();
        }

        /**
         * This method updates the score of a player after a hit
         */
        private void updatePlayerScore()
        {
            string sMessage = r_GameManager.PlayerOne.Score < 2 ? "(s)" : "s";

            labelFirstPlayer.Text = string.Format("{0}: {1} Pair{2}", r_FirstPlayerName, r_GameManager.PlayerOne.Score, sMessage);
            sMessage = r_GameManager.GetPlayerTwoScore() < 2 ? "(s)" : "s";
            labelSecondPlayer.Text = string.Format("{0}: {1} Pair{2}", r_SecondPlayerName, r_GameManager.GetPlayerTwoScore(), sMessage);
            Application.DoEvents();
        }

        /**
         * This method displays a message box asking the user if he wants to replay
         */
        private void askGameReplay()
        {
            string[] gameDetails = r_GameManager.GetEndGameDetails(r_Col, r_Row);
            string message = "";

            if (gameDetails[2].Equals(gameDetails[3]))
            {
                message = string.Format("{0} and {1} tied! with {2} points{3}{3}Would you like to play another game?",
                    gameDetails[0],
                    gameDetails[1],
                    gameDetails[2],
                    Environment.NewLine);
            }
            else
            {
                message = string.Format("The winner is {0} with {1} points!{4}The loser is {2} with {3} points{4}{4}Would you like to play another game?", 
                    gameDetails[0],
                    gameDetails[2],
                    gameDetails[1],
                    gameDetails[3],
                    Environment.NewLine);
            }
            
            if (MessageBox.Show(message, "Game Over!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MemoryGameForm newGameForm = new MemoryGameForm(r_FirstPlayerName, r_SecondPlayerName, string.Format("{0} x {1}", r_Col, r_Row), r_IsHuman);

                this.Hide();
                newGameForm.ShowDialog();
            }
            
            this.Close();
            Application.Exit();
        }

        /**
         * resets the clicked buttons to their upside down positions
         */
        private void cardTurnOver()
        {
            foreach (Button button in r_ClickedButtonList)
            {
                button.BackgroundImage = null;
                button.FlatStyle = FlatStyle.Standard;
                button.FlatAppearance.BorderSize = 0;
                button.Text = null;
                button.UseVisualStyleBackColor = true;
                button.Enabled = true;
            }
        }

        /**
         * This method disables all buttons that are cards that have not been matched
         */
        private void disableButtons()
        {
            foreach (Button button in r_UnmatchedButtonsList)
            {
                if (m_InternetConnected)
                {
                    button.Click -= new EventHandler(button_Click);
                }
                else
                {
                    button.Click -= new EventHandler(buttonNoInternet_Click);
                }
            }
        }

        /**
         * This method enables all buttons that are cards that have not been matched
         */
        private void enableButtons()
        {
            foreach (Button button in r_UnmatchedButtonsList)
            {
                if (m_InternetConnected)
                {
                    button.Click += new EventHandler(button_Click);
                }
                else
                {
                    button.Click += new EventHandler(buttonNoInternet_Click);
                }
            }
        }

        /**
         * This method changes the action of click to apply when no internet is available
         */
        private void noInternetGameFormat()
        {
            foreach (Button button in r_UnmatchedButtonsList)
            {
                button.Click -= new EventHandler(button_Click);
                button.Click += new EventHandler(buttonNoInternet_Click); 
            }
        }
    }
}
