using System.Windows.Media.Imaging;

namespace SevenStarsToolbox.Data
{
    class Block
    {
        public static readonly String DYNAMIC_KEY = "%name%";
        private String dynamicName;
        private BlockTypes blockType;
        public Block(String dynamicName, BlockTypes type)
        {
            this.dynamicName = dynamicName;
            this.blockType = type;
        }

        public String GetName(String dynValue)
        {
            return dynamicName.Replace(DYNAMIC_KEY, dynValue);
        }

        public BlockTypes GetBlockType()
        {
            return blockType;
        }

        public List<BitmapImage> GetBlockBlankTextures()
        {
            return new List<BitmapImage>();
        }
    }
}
