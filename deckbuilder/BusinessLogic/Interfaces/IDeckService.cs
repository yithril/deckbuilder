using deckbuilder.Models;

namespace deckbuilder.BusinessLogic.Interfaces
{
    interface IDeckService
    {
        Deck SaveDeck(Deck deck);
    }
}
