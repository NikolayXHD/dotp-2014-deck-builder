using System;
using System.Text;

namespace RSN.DotP
{
	public enum WadHeaderContentFlags
	{
		Invalid = 0,
		Avatar = 0x0001,
		Background = 0x0002,
		Campaign = 0x0004,
		Deck = 0x0008,
		Foil = 0x0010,
		Font = 0x0020,
		Glossary = 0x0040,
		LoadingScreen = 0x0080,
		PlayField = 0x0100,
		ReloadAll = 0x0200,
		Unlock = 0x0400,
	}

	public class WadHeaderInfo
	{
		public bool ForAppIdLinking;	// This is mainly here for backward compatibility reasons.
		public int ContentAppId;
		public int ContentPackId;
		public WadHeaderContentFlags ContentFlags;
		public int DeckId;

		public WadHeaderInfo()
			: this(0, 213850, 0, WadHeaderContentFlags.Deck | WadHeaderContentFlags.Glossary)
		{ }

		public WadHeaderInfo(int nContentPack, int nContentAppId, int nDeckId, WadHeaderContentFlags eFlags)
		{
			ContentPackId = nContentPack;
			ContentAppId = nContentAppId;
			DeckId = nDeckId;
			ContentFlags = eFlags;
		}
	}
}
