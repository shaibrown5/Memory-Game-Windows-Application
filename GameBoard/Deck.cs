using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;

namespace GameDeck
{
    public class Deck
    {
        // Fields
        private readonly List<Card> r_CardList;
        private readonly List<string> r_ListOfPicUrl = new List<string>();

        /*
        * The Cards constructor
        * Initializes the size of the list
        * Adding values to r_CardList and shuffling them.
        */
        public Deck(int i_NumberOfCards)
        {
            this.r_CardList = new List<Card>(i_NumberOfCards);

            makeCards(i_NumberOfCards);
            //shuffleCards();
        }

        /*
        * Build the deck of cards
        * Adding 2 one of each card
        */
        private void makeCards(int i_NumberOfCards)
        {
            string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R" };

            try
            {
                for (int i = 0; i < (i_NumberOfCards / 2); i++)
                {
                    Card card = new Card(letters[i], getRandomImage());
                    r_CardList.Add(card);
                    r_CardList.Add(card);
                }
            }
            catch (System.Net.WebException we)
            {
                makeNonPictureCards(i_NumberOfCards, letters);
            }
        }

        /**
         * Makes cards with letters instead of pictures, in the case that internet is not available
         */
        private void makeNonPictureCards(int i_NumberOfCards, string[] i_DataArray)
        {
            for (int i = 0; i < (i_NumberOfCards / 2); i++)
            {
                Card card = new Card(i_DataArray[i]);
                r_CardList.Add(card);
                r_CardList.Add(card);
            }
        }

        /**
        * gets a random image url from https://picsum.photos/80 and makes sure it is not in use already
        */
        private Image getRandomImage()
        {
            WebRequest request;
            WebResponse response;
            string url = "";

            do
            {
                request = WebRequest.Create("https://picsum.photos/80");
                response = request.GetResponse();
                url = response.ResponseUri.ToString();
            } while (r_ListOfPicUrl.Contains(url));

            r_ListOfPicUrl.Add(url);
            Stream imageStream = response.GetResponseStream();
            Image newImage = Image.FromStream(imageStream);

            return newImage;
        }

        /*
        * Shuffling the cards using Fisher-Yates algorithm
        */
        private void shuffleCards()
        {
            Random random = new Random();

            for (int i = r_CardList.Count - 1; i > 0; i--)
            {
                int randomIndex = random.Next(0, i + 1);
                Card holder = r_CardList[i];
                r_CardList[i] = r_CardList[randomIndex];
                r_CardList[randomIndex] = holder;
            }
        }

        /*
        * Get the wanted card from the list
        */
        public Card GetCard(int i_CardIndex)
        {
            return r_CardList[i_CardIndex];
        }
    }
}
