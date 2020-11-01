namespace deckbuilder.DataAccess.Interfaces
{
    using deckbuilder.Models;
    using System.Collections.Generic;

    interface IDeckBuilderRepository
    {
        void Save(Deck deck);
        void Delete(Deck deck);
        Deck Get(string id, string dataType);
        IEnumerable<Deck> ListByType(string dataType);
    }
}
