using System.Drawing;

namespace GameDeck
{
    public class Card
    {
        // Field
        private readonly string r_Data;
        private readonly Image r_CardImage = null;

        // Getter for card data
        public string Data
        {
            get
            {
                return this.r_Data;
            }
        }

        //getter for card Image
        public Image CardImage
        {
            get
            {
                return r_CardImage;
            }
        }

        /*
        * The Card constrcutor
        * Init the r_Data of the Card
        */
        public Card(string i_Data, Image i_Image)
        {
            this.r_Data = i_Data;
            this.r_CardImage = i_Image;
        }

        /**
         * Non picture constructor
         */
        public Card(string i_Data)
        {
            this.r_Data = i_Data;
        }
    }
}

