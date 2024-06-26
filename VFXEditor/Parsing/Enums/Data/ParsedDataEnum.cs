﻿using System;

namespace VfxEditor.Parsing.Data {
    public class ParsedDataEnum<T, S> : ParsedEnum<T> where T : Enum where S : class, IData {
        private readonly IItemWithData<S> Item;

        public ParsedDataEnum( IItemWithData<S> item, string name, T value, int size = 4 ) : base( name, value, size ) {
            Item = item;
        }

        public ParsedDataEnum( IItemWithData<S> item, string name, int size = 4 ) : base( name, size ) {
            Item = item;
        }

        public override void Update( T prevValue, T value ) {
            CommandManager.Add( new ParsedDataEnumCommand<T, S>( new ParsedSimpleCommand<T>( this, prevValue, value ), Item ) );
        }

        public override void Update( T value ) {
            CommandManager.Add( new ParsedDataEnumCommand<T, S>( new ParsedSimpleCommand<T>( this, value ), Item ) );
        }
    }
}
