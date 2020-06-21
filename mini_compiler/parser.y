%namespace GardensPoint

%union
{
	public string val;
	public SyntaxTree node;
}

%token Program If Else While Read Write Return Int Double Bool False True Assign Or And BitwiseOr BitwiseAnd Equals NotEquals
%token GreaterThan GreaterEqual LessThan LessEqual Plus Minus Mul Div Not BitwiseNot OpenPar ClosePar OpenCurly 
%token CloseCurly Semicolon

%token <val> Ident RealNumber IntNumber String

%type <node> program declaration 
%type <node> primary_expression unary_expression unary_operator bitwise_expression multiplicative_expression
%type <node> additive_expression relational_expression logical_expression expression
%type <node> statement declaration_list statement_list selection_statement write_statement main_statement block_statement

%%

program
	: Program main_statement
		{
			Compiler.tree = new Program(@1.StartLine, $2);
		}
	;

/*-------------------------------------------------------------------------------------------------*/

primary_expression
	: Ident
		{
			$$ = new PrimaryExp(@1.StartLine, PrimaryExpType.Ident, $1);
		}
	| False
		{
			$$ = new PrimaryExp(@1.StartLine, PrimaryExpType.False, "false");
		}
	| True
		{
			$$ = new PrimaryExp(@1.StartLine, PrimaryExpType.True, "true");
		}
	| RealNumber
		{
			$$ = new PrimaryExp(@1.StartLine, PrimaryExpType.RealNumber, $1);
		}
	| IntNumber
		{
			$$ = new PrimaryExp(@1.StartLine, PrimaryExpType.IntNumber, $1);
		}
	| OpenPar expression ClosePar
		{
			$$ = new PrimaryExp(@1.StartLine, PrimaryExpType.Exp, $2);
		}
	;

unary_expression
	: primary_expression
		{
			$$ = new UnaryExp(@1.StartLine, $1);
		}
	| unary_operator unary_expression
		{
			$$ = new UnaryExp(@1.StartLine, $1, $2);
		}
	;

unary_operator
	: Minus
		{
			$$ = new UnaryOp(@1.StartLine, UnaryOpType.Minus);
		}
	| BitwiseNot
		{
			$$ = new UnaryOp(@1.StartLine, UnaryOpType.BitwiseNot);
		}
	| Not
		{
			$$ = new UnaryOp(@1.StartLine, UnaryOpType.Not);
		}
	| OpenPar Double ClosePar
		{
			$$ = new UnaryOp(@1.StartLine, UnaryOpType.Cast2Double);
		}
	| OpenPar Int ClosePar
		{
			$$ = new UnaryOp(@1.StartLine, UnaryOpType.Cast2Int);
		}
	;

bitwise_expression
	: unary_expression
		{
			$$ = new BitwiseExp(@1.StartLine, $1);
		}
	| bitwise_expression BitwiseOr unary_expression
		{
			$$ = new BitwiseExp(@1.StartLine, $1, $3, BitwiseOpType.Or);
		}
	| bitwise_expression BitwiseAnd unary_expression
		{
			$$ = new BitwiseExp(@1.StartLine, $1, $3, BitwiseOpType.And);
		}
	;

multiplicative_expression
	: bitwise_expression
		{
			$$ = new MulExp(@1.StartLine, $1);
		}
	| multiplicative_expression Mul bitwise_expression
		{
			$$ = new MulExp(@1.StartLine, $1, $3, MulOpType.Mul);
		}
	| multiplicative_expression Div bitwise_expression
		{
			$$ = new MulExp(@1.StartLine, $1, $3, MulOpType.Div);
		}
	;

additive_expression
	: multiplicative_expression
		{
			$$ = new AddExp(@1.StartLine, $1);
		}
	| additive_expression Plus multiplicative_expression
		{
			$$ = new AddExp(@1.StartLine, $1, $3, AddOpType.Add);
		}
	| additive_expression Minus multiplicative_expression
		{
			$$ = new AddExp(@1.StartLine, $1, $3, AddOpType.Sub);
		}
	;

relational_expression
	: additive_expression
		{
			$$ = new RelExp(@1.StartLine, $1);
		}
	| relational_expression LessThan additive_expression
		{
			$$ = new RelExp(@1.StartLine, $1, $3, RelOpType.LessThan);
		}
	| relational_expression GreaterThan additive_expression
		{
			$$ = new RelExp(@1.StartLine, $1, $3, RelOpType.GreaterThan);
		}
	| relational_expression LessEqual additive_expression
		{
			$$ = new RelExp(@1.StartLine, $1, $3, RelOpType.LessEqual);
		}
	| relational_expression GreaterEqual additive_expression
		{
			$$ = new RelExp(@1.StartLine, $1, $3, RelOpType.GreaterEqual);
		}
	| relational_expression Equals additive_expression
		{
			$$ = new RelExp(@1.StartLine, $1, $3, RelOpType.Equals);
		}
	| relational_expression NotEquals additive_expression
		{
			$$ = new RelExp(@1.StartLine, $1, $3, RelOpType.NotEquals);
		}
	;

logical_expression
	: relational_expression
		{
			$$ = new LogExp(@1.StartLine, $1);
		}
	| logical_expression And relational_expression
		{
			$$ = new LogExp(@1.StartLine, $1, $3, LogOpType.And);
		}
	| logical_expression Or relational_expression
		{
			$$ = new LogExp(@1.StartLine, $1, $3, LogOpType.Or);
		}
	;

expression
	: logical_expression
		{
			$$ = new Exp(@1.StartLine, $1);
		}
	| logical_expression Assign expression
		{
			$$ = new Exp(@1.StartLine, $1, $3);
		}
	;

/*-------------------------------------------------------------------------------------------------*/

declaration
	: Int Ident Semicolon
		{
			$$ = new Decl(@1.StartLine, VarType.Int, $2);
		}
	| Double Ident Semicolon
		{
			$$ = new Decl(@1.StartLine, VarType.Double, $2);
		}
	| Bool Ident Semicolon
		{
			$$ = new Decl(@1.StartLine, VarType.Bool, $2);
		}
	;

/*-------------------------------------------------------------------------------------------------*/

statement
	: block_statement
		{
			$$ = new Stat(@1.StartLine, StatType.Block, $1);
		}
	| expression Semicolon
		{
			$$ = new Stat(@1.StartLine, StatType.Exp, $1);
		}
	| selection_statement
		{
			$$ = new Stat(@1.StartLine, StatType.Selection, $1);
		}
	| While OpenPar expression ClosePar statement
		{
			$$ = new Stat(@1.StartLine, StatType.While, $3, $5);
		}
	| Read Ident Semicolon
		{
			$$ = new Stat(@1.StartLine, StatType.Read, $2);
		}
	| write_statement
		{
			$$ = new Stat(@1.StartLine, StatType.Write, $1);
		}
	| Return Semicolon
		{
			$$ = new Stat(@1.StartLine, StatType.Return);
		}
	;

block_statement
	: OpenCurly CloseCurly
		{
			$$ = new BlockStat(@1.StartLine);
		}
	| OpenCurly statement_list CloseCurly
		{
			$$ = new BlockStat(@1.StartLine, $2);
		}
	;

main_statement
	: OpenCurly CloseCurly
		{
			$$ = new MainStat(@1.StartLine, MainStatType.Empty);
		}
	| OpenCurly statement_list CloseCurly
		{
			$$ = new MainStat(@1.StartLine, MainStatType.Stat, $2);
		}
	| OpenCurly declaration_list CloseCurly
		{
			$$ = new MainStat(@1.StartLine, MainStatType.Decl, $2);
		}
	| OpenCurly declaration_list statement_list CloseCurly
		{
			$$ = new MainStat(@1.StartLine, MainStatType.DeclStat, $2, $3);
		}
	;

declaration_list
	: declaration
		{
			$$ = new DeclList(@1.StartLine, $1);
		}
	| declaration_list declaration
		{
			$$ = new DeclList(@1.StartLine, $1, $2);
		}
	;

statement_list
	: statement
		{
			$$ = new StatList(@1.StartLine, $1);
		}
	| statement_list statement
		{
			$$ = new StatList(@1.StartLine, $1, $2);
		}
	;

selection_statement
	: If OpenPar expression ClosePar statement
		{
			$$ = new SelectionStat(@1.StartLine, $3, $5);
		}
	| If OpenPar expression ClosePar statement Else statement
		{
			$$ = new SelectionStat(@1.StartLine, $3, $5, $7);
		}
	;

write_statement
	: Write Ident Semicolon
		{
			$$ = new WriteStat(@1.StartLine, WriteStatType.Ident, $2);
		}
	| Write String Semicolon
		{
			$$ = new WriteStat(@1.StartLine, WriteStatType.String, $2);
		}
	;

%%

public Parser(Scanner scanner) : base(scanner) { }