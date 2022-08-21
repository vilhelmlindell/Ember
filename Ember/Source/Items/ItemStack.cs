namespace Ember.Items
{
    public class ItemStack
    {
        public Item Item;
        public int Count;

        public ItemStack(Item item, int count = 0)
        {
            Item = item;
            Count = count;
        }
    }
}
