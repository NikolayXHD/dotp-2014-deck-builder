--------------------------------------------------------------------------------
---- Riiak Shi Nal's Duels of the Planeswalkers 2014 Deck Builder v1.3.0.0 -----
--------------------------------------------------------------------------------

    This is the first Deck Builder for DotP 2014 and it will ask for the DotP
2014 game directory from which it will load most of the resources it will use.
It allows the user to build a deck from any cards they currently have installed
and helps them in building images for the deck, preview cards, and AI
Personality images as necessary.  It will also attempt to prevent users from re-
using deck uids and other little annoyances that can prevent one thing or
another from working.

I, Riiak Shi Nal, have tried to make this work as simply as possible while still
retaining a lot of the power that is given to modders.  There are some things
that work a bit differently than you might expect and there are items that have
some nice little features that you may not notice at first.  I will try to cover
those things in this document.

--------------------------------------------------------------------------------
Contents:
--------------------------------------------------------------------------------
- Main Features
- Requirements
- Getting Started
- Adding Cards
- Removing Cards
- Saving
- Exporting
- Exporting Images
- Id Schemes
- Building images
- Card, Deck, and Unlock Views
- Deck Statistics
- Land Config
- Bias & Promo Statuses
- Regular Unlocks
- Promo Unlocks
- Advanced Filtering
- Tools: Custom Data Folder & Creating a Core Wad
- Multi-Column Sorting
- Notes
- Planned Enhancements
- Considered Features
- Known Issues
- Frequently Asked Questions (FAQ)
- Credits & Thanks
- Change History
- Contact Information

--------------------------------------------------------------------------------
Main Features:
--------------------------------------------------------------------------------
- Does not require wads to be extracted/imported to use resources from them.
    Automatically uses all available game resources (cards, decks, unlocks,
    images, personalities, etc...) from the game directory you point it to.

- Automatically calculates deck colour (or lack thereof).

- Will assist the user in building appropriate images for deck boxes and AI
    personalities.

- Program itself is localizable (translations need to be finished though) so you
    can use the program in your selected language.

- Supports TDX compression of generated images.

- Will automatically create Land Pool XMLs based on the colours of the deck.

- Can create Deck Wads as either a packed WAD file or a specially set up
    directory complete with headers.

--------------------------------------------------------------------------------
Requirements:
--------------------------------------------------------------------------------
- .Net 4.0 Framework - This is the framework version that I compiled using.

- Modified Gibbed Tools (version r6_b10 modified, included) - I reference the
    gibbed tools libraries for parsing wads and tdx images.  I do not use the
    executables as I have created class wrappers that allow me to do the work I
    need to do in code rather than from the command line.  Source code from the
    executables was used rather heavily in the development of the wrappers.
    Gibbed Tools Squish and FileFormats have to be my modified versions for the
    compression of TDX images, the reason I can't just fall back on the original
    versions is because I specifically reference functions I added to make the
    compression features work.  Source code for the modified versions can be
    found here: http://www.slightlymagic.net/forum/viewtopic.php?f=99&t=10463

--------------------------------------------------------------------------------
Getting Started:
--------------------------------------------------------------------------------
    When you first open the Deck Builder for the first time it will state that
the game directory has not been set and needs to be set to continue.  Clicking
"OK" will bring up the "Options" window where you can either enter or select a
working game directory where the builder will load most of its resources from.

In this screen it is also highly recommended that you change the Id Scheme
options to help prevent unique id collisions with other modders (more on Id
Schemes later in this document).  Also if your native language is not English
you can also change the language that this program will run in.  Due note
however that since I only know English (and a little Japanese) that the only
localization I have been able to fully fill out is the English localization.

Once you have specified the game directory and clicked "Apply" the builder will
load all the wads (and "wad directories") from the game directory and populate
the card list at the bottom of the screen.  To keep things simple you can simply
double-click on a card and it will add it to either the deck or one of the
unlocks as per which radio button is selected above the card list.

You can edit the deck name (in all supported languages) by clicking the "Edit
Name" button above the deck cards list.  Much of the rest of the deck info can
be edited and/or managed by clicking the "Deck Information" button below the
deck cards list.  This includes the deck's availability, which card to make the
preview images from, the deck's uid (which will be modified by the Id Scheme),
deck statistics, land configuration, and building of the deck box image.

You can also save and open deck's using the file menu.  If you are looking at
the file menu you will also notice an option for creating a deck from an
existing deck.  This means that you can select one of the deck's that you
already have installed to work from as a base.  Note that creating a deck from
one you already have installed is NOT the same as editing a deck.  This builder
does not support deck "editing" only "building" (creating new decks).  There are
several reasons for this such as the problems inherent with users changing base
game wads and the chance that a user will overwrite something they shouldn't.

Once you are finished editing your deck you can "Export" the deck using the file
menu's "Export To" menu.  You have two options for exporting you can export as a
directory (which the game will read from like a wad if a wad with the same name
is not present) or as a wad (which will package up all the files generated in a
single compressed wad file).  Once generation is complete you will have both the
generated wad (or directory) and a fairly generic readme file for your deck.

--------------------------------------------------------------------------------
Adding Cards:
--------------------------------------------------------------------------------
    You can easily add cards by either hitting "Enter" while you have a card
highlighted in the main card list or by "Double-Clicking" the card you want to
add.  If you are adding cards to the main deck and you add a single card more
than once it will simply increment the quantity of that card that you have in
the main deck.  This does not apply if you have given a card in the deck a
special status such as Bias or Promo status.  When a card is added it is added
at a Bias of 1 and no Promo status so if there is a card in the deck with a Bias
of 2 or 3, or you have given that card Promo status then you add another copy of
that card from the main card list, the added card will show up in your list as a
separate instance with a Bias of 1 and no Promo status.

For the unlocks each card will always have a quantity of 1 because each unlock
needs to be a single card and you may want to stagger a few copies of a card
throughout the unlock list which can only really be done if all the cards have a
quantity of 1.

This builder does not put limits on the total number of cards that can be added
to a deck and its unlocks (except for Promo unlocks which has a max number of
ten cards).  This does mean that you could design a deck that has hundreds of
cards in it, but it probably won't work properly in game.  The game uses a
single byte for the deckOrderId and for keeping track of whether a card is in
the deck or not.  Practically this means that deckOrderIds of 0 through 127 are
available for use (128 card max) and this limit applies to the total number of
cards a deck has including the unlocks.

While the max card limit for decks to be used in game is 128 cards the game
itself is optimized for a 60 card deck and up to about 40 total unlocks (100
total cards) including the basic land in the main deck.

--------------------------------------------------------------------------------
Removing Cards:
--------------------------------------------------------------------------------
    To remove cards from the deck or from the unlocks make sure the card you
want to remove is highlighted, the focus is on that listing and hit delete on
the keyboard ("Del" key).  you can also right-click the card you want to remove
and select "Remove Card".

Note: You can't remove cards from the main card list (this program does not
    support changing existing wads).  If you simply want to filter which cards
    are shown in the main card list then use the "Set Filters" button and select
    which filters you want to use.

--------------------------------------------------------------------------------
Saving:
--------------------------------------------------------------------------------
    Saving operates a bit differently than you might expect.  It will save out
the deck, but the file generated when you save is not meant to be used by the
game.  It contains extra information so that the Deck Builder does not need to
manage multiple linked files.  For example all saved decks will also include the
land pool in a tag inside the deck.  All unlocks, strings, and images generated
will also be included inside the deck.

If you want to generate files that are appropriate for the game to use then you
need to "Export" the deck.

--------------------------------------------------------------------------------
Exporting:
--------------------------------------------------------------------------------
    When you export a deck it will automatically name the output wad (or
directory) based on the final deck uid and the English name of the deck codified
to be file system friendly.  So if you name your deck "My First Deck" and you
are using the default Id Scheme with an id of 0 then the name or your exported
wad will be "DATA_DECKS_100000_MY_FIRST_DECK.wad".  This is done this way for a
few reasons:
- Mods must start with either "DATA_DECKS_" or "DATA_DLC_" (case insensitive) to
    be recognized.
- Putting the Deck's uid in the wad name makes figuring out the uid of the deck
    inside very easy (this can help if looking for UID conflicts).
- Having the Deck name in the filename makes managing custom decks easier for
    the user because the name they see in game relates to the one they see on
    the wad.  Now there may be some differences for this due to language
    localizations, but this is a good compromise.

The exported wad (or directory) will automatically be put into the game
directory so that it is ready to use.  Also by having it in the game directory
it will also be loaded into the builder as an existing deck the next time the
builder is run.

--------------------------------------------------------------------------------
Exporting Images:
--------------------------------------------------------------------------------
    You can export images by right-clicking them and selecting "Export Image"
and choosing either PNG or TDX as the format to export as.  Images will be
exported as you see them there will be no further processing before export
(except to compress when saving to TDX).

The card preview you see in the main window (and view card window) without
border will compress to DXT1 format when saving to TDX, all other images will
compress to DXT5 when saving to TDX.

If you choose to "Export Card Previews" it will ask you to specify the save
location and filename for just the English preview image, it will take your
response and use that to determine where to save and how to name the other
localizations of the preview image.

--------------------------------------------------------------------------------
Id Schemes:
--------------------------------------------------------------------------------
    Id Schemes are something I came up with to ease making sure all deck, land
pool, and unlock ids are unique and don't conflict with other decks.  There are
three possible scheme types that can be used.  You can see previews of the ids
that will be generated by a scheme in the options window.

    Id Blocks are a new feature to Id Schemes that should make changing the Id
Scheme settings easier.  If you elect to use Id Blocks (true by default) then
you can select all four of the Change Ids (Deck, Land Pool, Regular, and Promo
Unlocks) by changing just the Id Block number.  This should also help users to
choose unique blocks more quickly.  Before you choose a block you should check
out the Prefix/Id Registry to make sure you choose one that will be unique to
you.

Prefix/Id Registry:
    http://www.slightlymagic.net/wiki/DotP_2014:_Prefix/Id_Registry

Additive Schemes - These schemes simply add a number to each of the ids to get
the next successive id.  A simple example would be that your for your deck you
choose id 1.  The land pool may then have id 2, regular unlocks id 3, and promo
unlocks could get id 4.  This is a very simplistic scheme, easy to understand
and use.

Prefix Schemes - These are my preferred schemes.  A prefix scheme puts a pre-
specified prefix in front of the various ids allowing for very unique ids in a
single large numeric block.  This is a well recognized scheme type that several
modders use.

The default scheme uses a modification on the scheme used by thefiremind
except I place it in the 1000 block instead of his 1999 block.  If you use
this scheme type I highly recommend changing it to put it into a block unique
to you.

Using the default scheme (prefix type) if you give a deck id 0 when exported
it will export with an id of 100001, the land pool will be 1000101, any
regular unlocks will have the id 1000201 and promo unlocks will get the id
1000301.

Suffix Schemes - These are similar to Prefix schemes except they append the
block to the end of the number instead of to the beginning.  Though it should
be noted that leading zeros are dropped (because that is how the game reads
them anyway) so if have a deck id of 0 and a deck suffix of 1000 when you
export you will get an id of 1000, not 01000.

--------------------------------------------------------------------------------
Building images:
--------------------------------------------------------------------------------
    When you start working with images either for the deck box or for the AI
Personality you will have three options.  You can use the current image if any.
Load an image (you will get an open dialog window when you select this option)
which will allow you to select an image you have already crafted for this
purpose.  Or you can build an image which will give you an open dialog much like
loading an image, but it will mask and overlay the image as necessary to create
a composited image suitable for use.

When you are building images you can adjust the image being masked by changing
the location and size numbers near the image or you can pan the image by left-
clicking and drag the mouse on the image.  You can also zoom in and out by
rotating the mouse wheel.

--------------------------------------------------------------------------------
Card, Deck, and Unlock Views:
--------------------------------------------------------------------------------
    On each of the Card, Deck, and Unlock views you can right click to get a
context menu that will allow you to hide (or unhide) the columns that are
visible for that view.

Additionally on any view that has card information a "View Card" option will
appear on the context menu which will give you a view of the current localized
card image and a complete listing of that card's XML.  You can also query which
decks if any are using a specific card by using the "Decks Used In" option.  The
program checks cards used in decks by filename so if a card is present in more
than one wad with the same filename it will locate all decks that use that
card's filename and not just those decks related to that wad.

--------------------------------------------------------------------------------
Deck Statistics:
--------------------------------------------------------------------------------
    The deck statistics you choose in the deck information screen (Size, Speed,
Flexibility, and Synergy) are for choosing what displays for those lines in the
deck manager in game.  While the game uses a 5 star system the decks themselves
use numbers from 0 to 10 and "?".  The question mark is allowed internally by
the program, but is not user selectable mainly because it shows up as 5 stars in
game just like the numeric value 10.  Whole stars are given for each even number
so a value of 2 is 1 star, 4 is 2 stars, 6 is 3 stars, etc....  If you select an
odd number the game will display the appropriate number of whole stars, plus a
half star.  So if you select 5 (the default for all statistics) it will show as
2 and a half stars in game.

--------------------------------------------------------------------------------
Land Config:
--------------------------------------------------------------------------------
    Land configurations are for the fine tuning of the land distributions for
decks.  If everything is left at defaults (Ignore CMC Over at -1, everything
else at 0) then the LandConfig tag will not be generated and the game will use
its standard method of determining what basic land to put in the deck.

The Minimum Land options are for determining the minimum number of that specific
land to add to the deck if it will remain at 60 cards or less.  If you have a
deck with 50 non-land cards and you specify 10 forests and 12 swamps then you
will not get that much land in game as those settings would put the deck over 60
cards.  The game will only add basic lands up to 60 cards, so these options are
more for fine-tuning ratios that the game will assign rather than the rule the
game will follow.

If you want to specify the exact land config the game will use (again following
the 60 card rule regarding lands explained in the paragraph above) then you can
set Ignore CMC Over to 0 and Number of Spells that Count as Land to 0 (this is
important as the game will subtract this number from your min land config) and
set the minimum land options to the number of lands you want.  Remember the game
will only add basic lands until you reach a total of 60 cards in the deck if it
reaches 60 cards before it has finished adding the land from your land config
then it will simply stop adding basic lands short of your config.

--------------------------------------------------------------------------------
Bias & Promo Statuses:
--------------------------------------------------------------------------------
    Bias is related to the shuffling of the AI's deck and the user's selected
difficulty level.  If a card is given a bias of greater than 1 then it will be
shuffled towards the bottom of the deck when the AI is using the deck if the
selected difficulty level is LESS than the set bias level.  A Bias of 2 will
shuffle that card towards the bottom of the deck if the difficulty level is set
to "Mage".  A Bias of 3 will shuffle the card towards the bottom of the deck if
the difficulty level is set to either "Mage" or "Archmage".  A difficulty of
"Planeswalker" will ignore any biased shuffling.

    Promo status can be set on a card by card basis and controls whether that
individual card is shown as a foil card with the "Promotion" set mark.

    Bias and Promo statuses can be set at the same time and appear to work as
expected in game.

--------------------------------------------------------------------------------
Regular Unlocks:
--------------------------------------------------------------------------------
    These are the normal type of unlocks that you see in game.  If you do not
have the promo codes entered these are the only unlocks that you will see in
game.  This operates in an easier fashion than it did in DotP 2013 because the
AppId linking information is now stored in the deck itself and the deck builder
automatically puts in the AppId for the base game to have the unlocks, unlocked
by default.

--------------------------------------------------------------------------------
Promo Unlocks:
--------------------------------------------------------------------------------
    These unlocks can be unlocked in game by typing in the 10 promo unlock codes
which will unlock a single card from the promo unlocks of every deck that is
installed.  There is a limit of 10 cards that can be unlocked this way though,
so once the promo unlocks have 10 cards in them you will be unable to add any
more to that type of unlock.  The deck builder will automatically switch to
putting the added cards into the regular unlocks.

--------------------------------------------------------------------------------
Advanced Filtering:
--------------------------------------------------------------------------------
    Advanced filtering will allow you to choose which filters specifically you
want to use for filtering the cards in the master list.  The filters are shown
using a heirarchical tree view with the root item always being a Filter Set.
All other filters can be added, replaced, and moved (via drag-and-drop) to get
the exact combination of filters that you want.

Of the filters available for you to choose from and use there are Filter Sets,
Boolean Filters, Integer Filters, String Filters, Enum Filters, and Power &
Toughness Filters.  All filters have a Next Filter comparison option that is
uses Boolean Logic (And/Or).  If you choose "And" then both filters must be true
for the card to be allowed.  With "Or" if either one of the two filters is true
then the card will be allowed.

Filters are processed sequentially with no short-circuiting of the logic.  This
means that it will process the first filter then "And" or "Or" the result with
the next filter in line until it reaches the final filter.  This also means that
if you have several filters "And"ed together that even if the first filter
returns false it will still test all the other filters.

Filter Sets: These provide no filtering by themselves, but can be used to group
    other filters for more fine-tuning.  Sets are great for grouping of
    conditions such as you might want to look for Creatures with the word
    "Destroy" in their abilities or you want to look for creatures with power
    greater than 2 and less than 7.  To do that you would group the two power
    filters so that they will be considered together and then they can be
    compared with the String filter as a group instead of individually.  There
    is no limit to the number of nested sets you can have.

Boolean Filters: These filter on a given boolean field of the Card being either
    true or false.  Instead of choosing True or False you actually choose the
    field to check and "Card is/has/can" or "Card is not/does not have/can't".
    So for example you could filter out tokens by saying "Card is not" "Token".

Integer Filters: These filters function like standard math inequalities.  You
    choose the field to compare on, the inequality operation to use (==
    [equals], >, >= <, <=, != [not equal]), and you choose the value to compare
    it to.  So for example you could look for cards with a Converted Mana Cost
    < 4 to find cards with a casting cost 3 or less.

String Filters: These filters use the standard string comparisons (Contains,
    Does Not Contain, Equal, and Not Equal) and function like a simple search.
    Though Advanced Filtering does allow you to filter on fields that the
    regular filtering does not allow (List of Registered Tokens, Artist,
    Expansion, Card XML).

Enum Filters: These filters work on fields that use Enumerations instead of
    strings or integers and will only allow you to filter on members of that
    enumeration.  For example you could select to filter on "Colour", "Is"
    "Multi-Coloured" to find all the multi-coloured cards, but you can't do
    "Colour", "Is", "Purple" because "Purple" is not a member of that enum.

Power/Toughness Filters: You're probably wondering why these are separate from
    Integer Filters, the reason is Power and Toughness have a valid value that
    is not an Integer and treating it as an integer would simply be wrong.  That
    value is "*" which in the world of Magic: the Gathering means that this
    value is variable and depends on what is written in the card's abilities.
    It could be something like Master of Etherium where its power and toughness
    are each equal to the number of artifacts you control or it could be like
    Maro where the power and toughness are each the number of cards in your
    hand.  The point is "*" is not a fixed number and cannot be treated as such,
    but otherwise these filters work just like the Integer Filters.

Note: Some of the advanced filters are somewhat slow and may take several
    seconds after being applied before the list will update.  This is due to
    several factors including that many of the Advanced Filters need to use .Net
    Reflection to operate, they may work on fields that have a lot of
    information like Card XML, and/or they need to do special processing on a
    field's data before it do the necessary checks like Sub-Types.

--------------------------------------------------------------------------------
Tools: Custom Data Folder & Creating a Core Wad:
--------------------------------------------------------------------------------
    The "Setup Custom Data Folder" option under the tools menu will create a
placeholder wad directory inside the game directory.  The created directory has
all the standard directory structure for a standard "Core" mod fleshed out with
an appropriate header file so that the game will recognize the contents of the
directory once the user starts adding data.  This allows the user to very easily
manage both custom cards in the builder and test them in game without having to
build a core wad each time to make sure the cards are updated.  The directory
created is always "DATA_DLC_DECK_BUILDER_CUSTOM", the user has no control over
that.

    Creating a core wad from the custom data stored in the custom data folder
can be done simply by using the "Create Core Wad from Custom Data" option in the
tools menu.  This takes all the data that the user has put into the custom data
folder (excluding empty directories) and writes it to a compressed wad that the
user chooses.  So if a user doesn't want to use Gibbed Tools to make a core wad
they can do so simply by using this option in the Deck Builder (does basically
the same thing Gibbed Tools would do).  The advantage this has over Gibbed Tools
is that it does not need an "unpacked" directory and it will re-create the
directory structure and header based on the filename being saved out rather than
requiring the user to set them.  For example the custom directory is
DATA_DLC_DECK_BUILDER_CUSTOM and the user decides to save it out as
Data_DLC_1000_Core.wad, the builder will take all the files (except HEADER.XML)
from the custom directory and create a new wad with the root directory being
DATA_DLC_1000_CORE and the header in the wad will be re-created to reflect this.

NOTE:  The deck builder still does not support building cards or any other files
not related to building a deck.  Other than a header XML the builder will not
put anything in the custom data folder.  This is NOT a substitute for manually
building/placing cards and their images.

--------------------------------------------------------------------------------
Multi-Column Sorting:
--------------------------------------------------------------------------------
    In the Card, Main Deck, and Create from Existing Deck views you can sort on
one or more columns.  To sort on a single column simply click the header of that
column, click again to reverse it.  To sort on multiple columns click on the
column header of the primary sort solumn, then hold "Shift" and click on the
header of each subsequent column you want to sort by.  You can reverse a sort on
a column in multiple column sort mode by holding "Shift" and clicking that
column's header.  To remove a column from a multiple column sort hold "Control"
(usually abreviated as CTRL on most keyboards) and click that column's header.

Your current sort will be saved for Card and Create from Existing Deck views.
They will also be restored when you go back to that view.

--------------------------------------------------------------------------------
Notes:
--------------------------------------------------------------------------------
- DotP 2014 has definitions for several deck strings including Deck tag (actual
    deck name), Deckname, Promos, Unlocks, and Description.  Though I've only
    been able to find where the Deck tag is used in-game, I can't see anywhere
    in-game where the others are used so I have omitted them from the builder at
    this time.  If someone can point out where they are used (maybe I'm just
    blind) then I'll be happy to add those fields into the builder.

- When upgrading from 1.1.0.0 or any previous version to v1.2.0.0 or later you
    will need to either delete your Settings.xml file, or manually go in and
    change the SortMode on the view columns from "Automatic" to "Programmatic"
    for the multiple column sorting to properly take effect.  Previous sort
    settings will not carry over from older versions.

--------------------------------------------------------------------------------
Planned Enhancements (in no particular order):
--------------------------------------------------------------------------------
None at this time.

--------------------------------------------------------------------------------
Considered Features (in no particular order):
--------------------------------------------------------------------------------
None at this time.

--------------------------------------------------------------------------------
Known Issues:
--------------------------------------------------------------------------------
- Due to font the type line on the cards may not be completely visible in some
    languages for some cards.

- For cards with a lot of text the program may not have enough room to draw on
    all the text.  As such card with a lot of text may have it cut off at the
    bottom of the preview image.  (This is most prevalent when looking at some
    of the planes.)

- Special code has not been added for the display of Planes, Schemes,
    Phenomenon, or Planeswalkers so if any are added/present they will probably
    not display correctly.  The game does not currently have any of these cards
    so I don't really expect a problem here.

- As of initial release English is the only complete localization all other
    languages are only partially complete.

--------------------------------------------------------------------------------
Frequently Asked Questions (FAQ):
--------------------------------------------------------------------------------
Q. Why does this program take so long to start up?
A. It takes a while because of all the data it is reading from the game
    directory to ensure that it is running with current information.  The more
    wads that are present in the game directory then the longer it is going to
    take to load this program.  Small wads load pretty quickly so having a lot
    of small deck wads is not usually a big problem.

Q. Why do the Id Scheme fields show with a pink background?
A. It is because you have either left the values at the defaults or you have
    set values that conflict with each other or with the defaults.  If every-
    thing is good then the fields will display with a light green background
    for prefix type Id Schemes or will display with the same background colour
    as the other fields.

Q. Why do the Id Scheme fields show with a green background?
A. It means everything looks good for those fields.  See above question for a
    more detailed answer.

Q. Why does this builder not have <insert feature>?
A. This can be due to several reasons including (but not limited to):
    - I had not thought about that feature.
    - The feature is not really technically feasible at the moment.
    - I specifically disallow it due to one reason or another.
    - I haven't gotten around to putting it in.

Q. Scrolling the card list is slow, why is that and how can I speed it up?
A. The slowest part of scrolling the card list in the default configuration is
    the image casting cost column, the images have to be built before the list
    can display them and it takes a small amount of time to build it (not an
    issue with 1 card, but when you start talking about hundreds to thousands it
    can eat up a good amount of time).  You can flip of the Image Casting Cost
    column and flip on the Text Casting Cost column (that will allow you to
    scroll faster).

Q. Can I make a 40 card deck?
A. No, the game will always add basic land until the deck reaches a total of 60
    cards.  This means if you put in 25 cards and leave room for 15 lands (basic
    or otherwise) the game will put in an additional 20 basic lands when you try
    and play it.  The program will keep track of how many total basic lands the
    deck will have (noted by the "Basic Land" counter above the main deck) so
    that you know how much basic land the deck will have in game.

Q. Can I make a deck with no basic land?
A. Yes, you most certainly can make a deck with no basic land (we expect the
    game to have no problem with this as DotP 2013 had no problem with it).
    This program will also have no problems if you create a deck with no basic
    land.

Q. Can I make a deck with more than 60 cards in it?
A. Yes, you can make a deck with more than 60 cards, but it should be noted that
    if you make a deck with more than 60 cards you will need to manually add the
    appropriate amount of land to the deck.  By default the builder will not
    show basic lands in the master card list, but you can show them by either
    allowing them in the filters window or by clearing all the filters (which
    will show all cards).

Q. Can I make a deck that has more than 4 copies of a given card in it?
A. Yes, the builder does not restrict how many of a specific card you put into a
    deck, so feel free to put in 10+ copies of Relentless Rats.  You should be
    aware though that if the card does not state that you can have more than the
    standard number of copies in a deck, then you will be in violation of MtG
    constructed deck building rules if you put in more than 4 copies.

Q. Why does the deck image show up as white box in the game?
A. This is because you did not set/create it in the Deck Information.

Q. Why do the AI Personality images show up as white boxes in game?
A. See above answer.

Q. Do I have to create an AI Personality?
A. No, you can use one of the many that have already been created, but creating
    one for your deck is a nice touch.

Q. Can I add my own music mixes to use for personalities?
A. Yes, but this program can't really help you with that.  All of the music goes
    into the Audio\Music directory under the main game directory and it needs to
    be in MP3 format.  Once it is in the directory then you can choose it for
    your custom personalities, though if you distribute those personalities then
    you need to make sure you also include that music file as well (it will not
    be put into the WAD).

Q. Do you expect this program to completely replace the way modders make decks?
A. No, modders tend to be creatures of habit (as is human nature), unless they
    personally see value in changing their methods they are unlikely to change
    the way they make mods.  Additionally this program does not do some things
    that regular modders might want, like allow for editing of cards, creating
    of cards, etc....  This program is intended mainly for non-modders and those
    who are looking to get into modding.

Q. Is this builder bug free?
A. Probably not. I hope it is free from major bugs as I put in a fairly decent
    amount of time testing it, but it is virtually impossible to make a bug free
    program.

Q. I have a wad that has cards in it that are not being loaded by the builder,
    why is this happening?
A. There are a few reasons this can happen including:
    - The card has a bad XML structure and has caused the loading of that card
        to error out. (Forgot to close a tag, typo, etc....)
    - The card either does not have all required tags or has an incorrect
        required attribute in one of the tags.
    - The wad it is in is not being loaded because it does not start with
        "DATA_DECKS_" or "DATA_DLC_" (which means it's also not read by the
        game).
    - That card hits a bug in my code.  If this is the case please let me know.

Q. Can I use this program to build cards?
A. No, this is a deck builder, it is designed to build decks not cards.

Q. Is it possible this could be modified to build cards as well as decks?
A. Sure, but that is a huge amount of changes I'm not yet prepared to make.
    Even if those changes were made it would still have the same limitations of
    other card generators in that it would only be able to fully generate
    simple cards and more complex cards/abilities would have to be manually
    coded.

Q. Why are there multiple cards with the same name and filename in the card
    listing?
A. This is because the builder loads all cards from all wads, this means if a
    card is present in multiple wads it will show multiple times in the card
    listing.

Q. I've saved a deck, why is it not showing in game?
A. You have to "Export" a deck for it to show up properly in game.

Q. I can't export because the menu option is grayed-out, what's wrong?
A. The builder determined that it did not have write access to the game
    directory and therefore there is no point in trying to export because it
    will fail.  You can try running the Deck Builder as Administrator to give it
    the permissions it needs to export.

Q. When I export it gives me a message saying a Uid conflict has been detected,
    what does this mean?  How can I stop this from happening?
A. This means that based on the settings of the Id scheme it has found that the
    Id you chose for the deck (or one of the files that will be created) is
    already being used by another deck, unlock file, or land pool and this can
    cause problems in the game if you try to run the game while the conflict
    exists.  This can be easily rectified by either going into Deck Information
    and clicking "Get Next Available Id" or by changing your Id Scheme settings
    to something more unique to you.  By default the Id Scheme settings put the
    ids in the 1000 prefixed block, so you could decide you want to put your
    decks in the 9834 prefixed block or 2358 prefixed block.  With about two
    billion possible uids it should be fairly easy to find some combination of
    settings that will be unique to you.

Q. Will the builder create "independent" wads when it exports?
A. No, it will not try to create an "independent" wad at this time as that
    would mean duplicating cards, images, and all function files from all wads
    that this one would be dependent on.  This could be done, but I opted not to
    because this could lead to old and/or conflicting function files/cards being
    used which could make people believe there is something wrong with your mod.
    It is ultimately best to keep the number of duplicate cards to a minimum.

Q. When exporting will it compress the images it creates?
A. Yes, the builder will now compress the images it creates to DXT5 compressed
    TDX files.

Q. I only speak one language, do I have to enter strings in all languages?
A. No, the Deck Builder will try to auto-fill any blank strings with the
    English string.  If possible you should always include an English string.
    Failure to include an English string could cause odd behavior such as
    generating a wad like: DATA_DECKS_100000_.wad and lacking names for strings
    for other missing languages.

Q. I can sort the cards in the card listing and for the deck cards, why can't
    I sort the cards in the unlocks?
A. The order in which cards appear in the deck is unimportant, but the order in
    which cards appear in the unlocks is important as it controls which
    unlockOrderId a card will get when exporting and can control when you get a
    card in game.  For example if you have 10 promo unlock cards, but only 8
    promo codes then you will only get 8 of the promo unlocks with the last two
    remaining locked.

Q. I don't like the template images you use for building the deck box image and
    the AI Personality images, can I change them?
A. Yes, you can, but there are a few things you need to be aware of:
    - The Overlay and Mask images MUST be the same dimensions to work properly.
    - The images must have the appropriate file names (case does not matter, at
        least in Windows).
    - The Mask does not properly support variable Alpha values.  If you use
        variable alpha values then you will get part of the mask in the final
        image (which will be visible when building an image).
    - The Mask should use a colour that is not likely to be in any chosen user
        image as any pixel that is found using that colour will be made
        transparent.
    - The top left most pixel in the mask is what is used to determine the mask
        colour so it should always be of the colour you want to be transparent.
    - The initial resizing and positioning of chosen images is based on my
        templates so if you change them you will likely need to adjust the
        position and size when building images to match your templates.
    - If your template and mask images are not sized in Multiples of Four (MoF)
        then the images built from them may get stretched or shrunk to the
        closest MoF size.  If I can't find a MoF size with a width 200 pixels
        in either direction I will resize to 512x512 and horribly stretch the
        image to fit.

Q. Why did the deck builder generate an "Errors.log" file?
A. This file is generated any time the program runs into any error that I don't
    consider a zero priority error.  Sometimes looking at this file can give
    you extra information on certain things.  For example you try to load a
    modder's deck that is supposed to have 36 non-basic-land cards in it, but
    it only loads 34 cards if you look at the error log you might see an entry
    that states that it "Can't find referenced card" for a deck which could
    indicate that the card is missing (for example a typo in the deck XML) or
    that there is a problem with the card (malformed XML, etc...).

Q. The Deck Builder generated an error log, will this cause problems?
A. In most cases, no, this will not cause a problem.  Though if the error log
    is generated then you should look at the log to see what errors were
    encountered.  If you see errors with "High" or "Critical" priorities then
    please report them to me because it means something really bad happened and
    I need to figure out what.  If you see "Low" or "Medium" errors then that
    means something couldn't be loaded or there is something important that
    needs your attention (bad card XML, a serious UID conflict, couldn't find a
    card that was referenced by a deck or unlocks, etc...).

Q. The readme for the Deck Builder is only in English why didn't you make one
    for each of the supported languages?
A. Because I only truly understand English (and a little bit of Japanese) so I
    can't properly translate the readme to each of those languages.  This is
    also why the other localizations of this program are only partial when I
    released this builder.

Q. The error log is in English, but I've selected that the program run in
    <Insert Language here>, how can I change that?
A. You can't, I purposely made the program so that the error log will always be
    in English as that is my native language and I am the developer.  This way
    if someone is reporting a bug and I request the error log I will be able to
    understand what the log is telling me.

Q. What if you disappear off the forums or stop developing this program?  How
    will I get support or will bugs get fixed?
A. I've released the full source code of this program so any programmer should
    be able to pick this up and examine the code, make bug fixes, updates, or
    even release their own version of this program.  I only ask that credit be
    given where credit is due.

Q. Will this work with all versions of Duels of the Planeswalkers?
A. No, it only works for Duels of the Planeswalkers 2014.  Each version of the
    game changes bits and pieces that are required for modding so if you want
    this for an older version of the game you're going to need to make the
    changes yourself.  Though there is a version available for DotP 2013:
        http://mtg.dragonanime.org/index.php?title=DotP_2013:_Deck_Builder

Q. There are some cards that just will not show up in the card list until I
    clear all the filters, why does this happen?
A. Some cards like invisible tokens will not show in the list because they have
    no types.

Q. I have an idea for a great new feature, how do I request it?
A. The best way is to mention it in the release thread for utility.  If I agree
    that it is a great idea then I'll likely add it to the list of planned
    improvements.

Q. Can I use parts of your code in my own programs?
A. Yes, give credit where credit is due and the program needs to be open-source
    and freely available.

Q. Is Riiak Shi Nal your real name?
A. No, but it is my preferred handle and it is highly unique.  If you see a
    Riiak Shi Nal or Riiak elsewhere then likely that person is also me, but if
    you want to confirm it then ask me either via PM or E-Mail.  Before I
    started using this handle many years ago I did a Google search on it and it
    came up with no results.  Now though there are several thousand more results
    for "Riiak" and little more than a thousand for "Riiak Shi Nal".

--------------------------------------------------------------------------------
Credits & Thanks (in no particular order):
--------------------------------------------------------------------------------
Tim Van Wassenhove - SortableBindingList & PropertyComparer
    http://www.timvw.be/2007/02/22/presenting-the-sortablebindinglistt/

Rick - Gibbed Tools & Source
    http://svn.gib.me/builds/duels/

thefiremind - Provided Italian localization.

Vulasuw - Provided Portuguese-Brazil localization.

Scion of Darkness - Provided shading layer and planeswalker symbol for the Deck
    Box template.
    http://www.slightlymagic.net/forum/viewtopic.php?f=109&t=10970

Microsoft - Visual Studio 2010

Wizards of the Coast - Magic: The Gathering & Duels of the Planeswalkers

Huggybaby - Slightly Magic forums & for being a great admin/moderator.
    http://www.slightlymagic.net/forum/

kevlahnota, thefiremind, and all other modders - For providing lots of mods and
    overcoming many technical difficulties regarding modding the DotP games.

Mythial - Beta testing for the 2013 version of this program.

BloodReyvyn & drleg3nd - Reporting bugs they found in the 2013 & 2014 versions.

Everyone else on the forums - For keeping the community alive and fun.

--------------------------------------------------------------------------------
Change History:
--------------------------------------------------------------------------------
- v1.3.0.0
    - Changed the Deck Availability back to Always Available now that we have
        confirmed that the problem only happens with content pack 0 and the Deck
        Builder now puts all the decks it creates into a content pack equal to
        the Id Block the user is using.
    - Dropped the opacity of the planeswalker symbol in the Deck Box Overlay by
        20 points (now 50% opacity instead of 70%).
    - Fixed a couple of bugs with not localizing the export menu options on the
        context menus for the main window.
    - Added context menu option to export card XML.  Note: Regardless of how the
        original file is stored it will save out the card XML as UTF-8 with BOM.
    - Added context menu options to export the cropped card image (the actual
        card image before any frames, boxes, or other processing is done) to
        either PNG or TDX format (if saving as TDX it should be an exact copy of
        what is stored).
    - Added 4 strings to localizations to support the new features.

- v1.2.0.0
    - Changed CardInfo class constructor to take in the actual filename of the
        card and store it for later checks.
    - Added ActualFilename to the CardInfo class, it is now possible to add this
        property to the card view columns, though I have not done so by default.
    - Added mismatched filename check to cards so the builder will now report
        when a card has a mismatched actual filename and FILENAME tag.
    - Fixed a rounding bug when calculating the closest MoF size.
    - Added a status bar with the total loaded cards and the cards currently in
        the card list (after filtering).
    - Changes to SortableBindingList to support sorting on multiple properties.
        Basically, I implemented IBindingListView for advanced sorting (not
        filtering).
    - Added the ability to sort on multiple columns (at least for those views in
        which it is possible to sort).
    - Moved AddViewColumn to Tools so I could use it on the Create from Existing
        Deck window.
    - Added 2 strings to localization files to support the new status strip at
        the bottom of the main window.

- v1.1.0.0
    - Changed default availability to locked due to DotP 2014 problem with lots
        of decks with always_available="true" set (probably within the same
        content pack).
    - Added WadHeaderInfo & WadHeaderContentFlags to store and handle the info
        for the header to create a new content_pack, assign the deck to it, and
        write all this information to the header to try and prevent a crash in
        the game.   The content pack that will be generated is equal to the
        value you specified for the IdBlock (if you are using the IdBlock other-
        wise it will be equal to the DeckIdChange).
    - When exporting it will now generate a "Content Pack Enabler" for the
        current content pack (currently identical to the Id Block).
    - Couple of minor changes to Wad naming, now the created Wads/Directories
        will have "Data_DLC_" and "Data_Decks_" instead of "DATA_DLC_" and
        "DATA_DECKS_" simply because I think it looks slightly better this way.

- v1.0.2.0
    - Added creation of MipMap chain to the compression of TDX images (enabled
        by default).
    - Added Option of whether to generate MipMaps to the Options window.
    - Fixed a language display bug on the Basic Filters window (Multi-Coloured
        was not showing properly).
    - Added 1 string to localization files to support the new MipMap option.

- v1.0.1.0
    - Fixed a bug with the individual Id Change fields being editable with the
        use Id Block checkbox checked.
    - Fixed an enabled bug with Export Card Previews being enabled even when a
        card wasn't selected.
    - Changed how the image files names were stored to eliminate a problem if
        a mod has an image present with the same name as another image of a
        different type (different location) in the mod.

- v1.0.0.0 - Initial release for DotP 2014 version. Changes from 2013 version:
    - Deck updated with new attributes.
    - AiPersonality updated with new tags.
    - AI Personality editing moved to new window (accessible from the Deck
        Information window).
    - ImageBuilder updated to allow adding an Alpha layer which will replace the
        built image's alpha layer (allows for smoother edges and required for
        the new Lobby images [Backplate]).
    - Changed method of loading TDX Images such that if there are multiple
        images with the same name in different directories the builder can now
        differentiate between them.
    - Removed all code for AppId Linking as DotP 2014 doesn't use it.
    - Removed Card Preview Image select from Deck Information as DotP 2014 does
        not use card preview images.  Card Previews can still be generated from
        the main card list using the context menu.
    - Automatic generating of random land pool decks.  The builder will create
        a land pool based on the colour(s) of the deck by picking 4 random land
        cards from the basic land pool for that colour and putting it into the
        land pool deck.  In the case of a colourless deck it will generate a
        full random land pool deck with lands from all five colours.
    - Added settings for Basic Land Pools (no editing of land pool decks yet
        though).  The new settings allow for the pool of basic lands from which
        the deck builder will automatically build land pool decks with to be
        changed.
    - Added additional location to search for images to support the new location
        for mana images.
    - Added IdBlock to the IdScheme settings, this allows for choosing IdScheme
        settings faster, and if it is set to use the IdBlock it will use this
        for determining the filename for the Core file when the user chooses to
        create one.  Core will still use the DATA_DLC_ prefix as the default for
        creating a core wad.
    - Added settings for Default Unlock & Foil Ids for Steam, iOs, & Android.
    - Added support for the Miracle card frames.
    - Changed LanguageStrings to add a prefix to the language code for loading
        purposes.  This allows for me to have different file names for different
        applications so I can host the language files directly from my Wiki.
    - Image templates updated to build images that match with the official Deck
        and Personality images.  (They can still be changed as described in the
        FAQ above.)
    - Updated export to remove the Card Preview images and add the new AI
        Personality Lobby image.
    - Updated LoadMusic to pull from the new music directory and check them as
        MP3s instead of OGGs.
    - Changes to what files/directories are read in when loading data from the
        game directory.
    - Changed what name is given to Decks when exported.  Now uses the
        DATA_DECKS_ prefix as DECK_ is no longer supported in DotP 2014.
    - Changed project name, and Guids so that it will truly be considered a
        separate project, in a separate assembly so that .Net won't simply
        consider it a different revision of the same program.
    - If using the IdBlock in the IdScheme settings then AI Personalities will
        now include the Block Id to make things even more unique (less chance of
        conflicts).

--------------------------------------------------------------------------------
Contact Information:
--------------------------------------------------------------------------------
If you want to contact me, you must be able to use English (bad or broken
English is okay if I can figure out what you are trying to say).  If you try to
contact me with any other language I will not respond or even try to figure out
what you are saying.

For the most part I prefer to be contacted on the Slightly Magic forums where my
handle is RiiakShiNal.  I will however answer bug reports and program comments
at this e-mail address (do NOT contact me for card or deck requests):
Riiak (at) DragonAnime (dot) org

Note: If you contact me via e-mail you MUST include a descriptive subject or I
    will immediately delete your e-mail.  Preferrably you should prefix the
    subject with "MtG:", "DotP:", or "Deck Builder:".