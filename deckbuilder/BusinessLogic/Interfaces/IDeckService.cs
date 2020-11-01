using deckbuilder.Models;

namespace deckbuilder.BusinessLogic.Interfaces
{
    public interface IDeckService
    {
        Deck SaveDeck(Deck deck);
    }
}
