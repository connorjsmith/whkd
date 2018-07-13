parser grammar whkdParser;
options {tokenVocab=whkdLexer;}

profile
    : binding+
    ;

binding
    : chords commandPiece+
    ;
chords
    : (chord ChordSeperator)* chord 
    ;
chord
    : ((key | choiceKey) ModifierSeperator)* (key | choiceKey)
    ;

key
    : Modifier
    | TerminalKey
    ;
// Matches {NonChoicechord1, NonChoicechord2, NonChoicechord3}
// Which means you cannot have a choice inside a choice ie. {a,b,{d,e,f}}
choiceKeyList
    : ChordListOpen ((Modifier ModifierSeperator)* TerminalKey ChordListSeperator)* (Modifier ModifierSeperator)* TerminalKey ChordListClose
    ;
choiceKey
    : (KeyChoiceName? choiceKeyList)
    ;

commandPiece
    : commandSinglePiece
    | commandChoicePiece
    ;
// TODO need to preserve whitespace in command mode
commandSinglePiece
    : CommandText
    ;
// Matches {Command1, Command2, Command3 ... CommandN}
commandList
    : CommandListOpen (commandSinglePiece CommandListSeperator)* commandSinglePiece CommandListClose
    ;
commandChoicePiece
    : (CommandChoiceName? commandList)
    ;
