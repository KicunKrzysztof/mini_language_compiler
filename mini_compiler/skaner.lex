%using QUT.Gppg;
%namespace GardensPoint

IntNumber		(0)|([1-9][0-9]*)
RealNumber		(0)|([1-9][0-9]*)\.[0-9]+
Ident			[A-Za-z][A-Za-z0-9]*
Comments		\/\/(.*)$
Whitespaces		[ \t\r]+
Newline			\n
String			\"((.|\\\")*)\"

%%
"program"			{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Program; }
"if"				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.If; }
"else"				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Else; }
"while"				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.While; }
"read"				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Read; }
"write"				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Write; }
"return"			{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Return; }
"int"				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Int; }
"double"			{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Double; }
"bool"				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Bool; }
"false"				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.False; }
"true"				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.True; }
"="					{ Console.WriteLine("="); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Assign; }
"||"				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Or; }
"&&"				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.And; }
"|"					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.BitwiseOr; }
"&"					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.BitwiseAnd; }
"=="				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Equals; }
"!="				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.NotEquals; }
">"					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.GreaterThan; }
">="				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.GreaterEqual; }
"<"					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.LessThan; }
"<="				{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.LessEqual; }
"+"					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Plus; }
"-"					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Minus; }
"*"					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Mul; }
"/"					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Div; }
"!"					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Not; }
"~"					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.BitwiseNot; }
"("					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.OpenPar; }
")"					{ Console.WriteLine("program"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.ClosePar; }
"{"					{ Console.WriteLine("{"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.OpenCurly; }
"}"					{ Console.WriteLine("}"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.CloseCurly; }
";"					{ Console.WriteLine(";"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); return (int)Tokens.Semicolon; }
{Newline}			;
{Ident}				{ Console.WriteLine("identyfikator"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return(int)Tokens.Ident; }
{RealNumber}		{ Console.WriteLine("real num"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return(int)Tokens.RealNumber; }
{IntNumber}			{ Console.WriteLine("int num"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return(int)Tokens.IntNumber; }
{String}			{ Console.WriteLine("string"); yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); yylval.val=yytext; return(int)Tokens.String; }
{Comments}			{ Console.WriteLine("komentarz");}
{Whitespaces}		;


%%
