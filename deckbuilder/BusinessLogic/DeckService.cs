using deckbuilder.BusinessLogic.Interfaces;
using deckbuilder.DataAccess.Interfaces;
using deckbuilder.Models;
using System;

namespace deckbuilder.BusinessLogic
{
    public class DeckService: IDeckService
    {
        private readonly IDeckBuilderRepository _deckBuilderRepository;

        public DeckService(IDeckBuilderRepository deckBuilderRepository)
        {
            _deckBuilderRepository = deckBuilderRepository;
        }

        public Deck SaveDeck(Deck deck)
        {
            try
            {
                _deckBuilderRepository.Save(deck);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save Doc with error {ex.Message}");
            }

            return deck;
        }
    }
}
