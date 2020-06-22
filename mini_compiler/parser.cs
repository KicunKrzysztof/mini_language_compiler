// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  DESKTOP-F5UB616
// DateTime: 6/22/2020 2:00:52 AM
// UserName: Krzys
// Input file <..\..\parser.y - 6/21/2020 4:06:50 PM>

// options: lines gplex

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using QUT.Gppg;

namespace GardensPoint
{
public enum Tokens {error=2,EOF=3,Program=4,If=5,Else=6,
    While=7,Read=8,Write=9,Return=10,Int=11,Double=12,
    Bool=13,False=14,True=15,Assign=16,Or=17,And=18,
    BitwiseOr=19,BitwiseAnd=20,Equals=21,NotEquals=22,GreaterThan=23,GreaterEqual=24,
    LessThan=25,LessEqual=26,Plus=27,Minus=28,Mul=29,Div=30,
    Not=31,BitwiseNot=32,OpenPar=33,ClosePar=34,OpenCurly=35,CloseCurly=36,
    Semicolon=37,Ident=38,RealNumber=39,IntNumber=40,String=41};

public struct ValueType
#line 4 "..\..\parser.y"
{
	public string val;
	public SyntaxTree node;
}
#line default
// Abstract base class for GPLEX scanners
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

// Utility class for encapsulating token information
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public class ScanObj {
  public int token;
  public ValueType yylval;
  public LexLocation yylloc;
  public ScanObj( int t, ValueType val, LexLocation loc ) {
    this.token = t; this.yylval = val; this.yylloc = loc;
  }
}

[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public class Parser: ShiftReduceParser<ValueType, LexLocation>
{
#pragma warning disable 649
  private static Dictionary<int, string> aliases;
#pragma warning restore 649
  private static Rule[] rules = new Rule[61];
  private static State[] states = new State[111];
  private static string[] nonTerms = new string[] {
      "program", "declaration", "primary_expression", "unary_expression", "unary_operator", 
      "bitwise_expression", "multiplicative_expression", "additive_expression", 
      "relational_expression", "logical_expression", "expression", "statement", 
      "declaration_list", "statement_list", "selection_statement", "write_statement", 
      "main_statement", "block_statement", "$accept", };

  static Parser() {
    states[0] = new State(new int[]{4,3},new int[]{-1,1});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{35,5},new int[]{-17,4});
    states[4] = new State(-2);
    states[5] = new State(new int[]{36,6,35,11,38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54,5,72,7,79,8,84,9,88,10,93,11,101,12,104,13,107},new int[]{-14,7,-13,96,-12,95,-18,10,-11,15,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50,-15,71,-16,87,-2,110});
    states[6] = new State(-49);
    states[7] = new State(new int[]{36,8,35,11,38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54,5,72,7,79,8,84,9,88,10,93},new int[]{-12,9,-18,10,-11,15,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50,-15,71,-16,87});
    states[8] = new State(-50);
    states[9] = new State(-56);
    states[10] = new State(-40);
    states[11] = new State(new int[]{36,12,35,11,38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54,5,72,7,79,8,84,9,88,10,93},new int[]{-14,13,-12,95,-18,10,-11,15,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50,-15,71,-16,87});
    states[12] = new State(-47);
    states[13] = new State(new int[]{36,14,35,11,38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54,5,72,7,79,8,84,9,88,10,93},new int[]{-12,9,-18,10,-11,15,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50,-15,71,-16,87});
    states[14] = new State(-48);
    states[15] = new State(new int[]{37,16});
    states[16] = new State(-41);
    states[17] = new State(new int[]{18,18,17,67,16,69,37,-35,34,-35});
    states[18] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-9,19,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[19] = new State(new int[]{25,20,23,42,26,58,24,60,21,62,22,64,18,-33,17,-33,16,-33,37,-33,34,-33});
    states[20] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-8,21,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[21] = new State(new int[]{27,22,28,44,25,-26,23,-26,26,-26,24,-26,21,-26,22,-26,18,-26,17,-26,16,-26,37,-26,34,-26});
    states[22] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-7,23,-6,56,-4,55,-3,28,-5,50});
    states[23] = new State(new int[]{29,24,30,46,27,-23,28,-23,25,-23,23,-23,26,-23,24,-23,21,-23,22,-23,18,-23,17,-23,16,-23,37,-23,34,-23});
    states[24] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-6,25,-4,55,-3,28,-5,50});
    states[25] = new State(new int[]{19,26,20,48,29,-20,30,-20,27,-20,28,-20,25,-20,23,-20,26,-20,24,-20,21,-20,22,-20,18,-20,17,-20,16,-20,37,-20,34,-20});
    states[26] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-4,27,-3,28,-5,50});
    states[27] = new State(-17);
    states[28] = new State(-9);
    states[29] = new State(-3);
    states[30] = new State(-4);
    states[31] = new State(-5);
    states[32] = new State(-6);
    states[33] = new State(-7);
    states[34] = new State(new int[]{12,37,11,39,38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-11,35,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[35] = new State(new int[]{34,36});
    states[36] = new State(-8);
    states[37] = new State(new int[]{34,38});
    states[38] = new State(-14);
    states[39] = new State(new int[]{34,40});
    states[40] = new State(-15);
    states[41] = new State(new int[]{25,20,23,42,26,58,24,60,21,62,22,64,18,-32,17,-32,16,-32,37,-32,34,-32});
    states[42] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-8,43,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[43] = new State(new int[]{27,22,28,44,25,-27,23,-27,26,-27,24,-27,21,-27,22,-27,18,-27,17,-27,16,-27,37,-27,34,-27});
    states[44] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-7,45,-6,56,-4,55,-3,28,-5,50});
    states[45] = new State(new int[]{29,24,30,46,27,-24,28,-24,25,-24,23,-24,26,-24,24,-24,21,-24,22,-24,18,-24,17,-24,16,-24,37,-24,34,-24});
    states[46] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-6,47,-4,55,-3,28,-5,50});
    states[47] = new State(new int[]{19,26,20,48,29,-21,30,-21,27,-21,28,-21,25,-21,23,-21,26,-21,24,-21,21,-21,22,-21,18,-21,17,-21,16,-21,37,-21,34,-21});
    states[48] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-4,49,-3,28,-5,50});
    states[49] = new State(-18);
    states[50] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-4,51,-3,28,-5,50});
    states[51] = new State(-10);
    states[52] = new State(-11);
    states[53] = new State(-12);
    states[54] = new State(-13);
    states[55] = new State(-16);
    states[56] = new State(new int[]{19,26,20,48,29,-19,30,-19,27,-19,28,-19,25,-19,23,-19,26,-19,24,-19,21,-19,22,-19,18,-19,17,-19,16,-19,37,-19,34,-19});
    states[57] = new State(new int[]{29,24,30,46,27,-22,28,-22,25,-22,23,-22,26,-22,24,-22,21,-22,22,-22,18,-22,17,-22,16,-22,37,-22,34,-22});
    states[58] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-8,59,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[59] = new State(new int[]{27,22,28,44,25,-28,23,-28,26,-28,24,-28,21,-28,22,-28,18,-28,17,-28,16,-28,37,-28,34,-28});
    states[60] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-8,61,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[61] = new State(new int[]{27,22,28,44,25,-29,23,-29,26,-29,24,-29,21,-29,22,-29,18,-29,17,-29,16,-29,37,-29,34,-29});
    states[62] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-8,63,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[63] = new State(new int[]{27,22,28,44,25,-30,23,-30,26,-30,24,-30,21,-30,22,-30,18,-30,17,-30,16,-30,37,-30,34,-30});
    states[64] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-8,65,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[65] = new State(new int[]{27,22,28,44,25,-31,23,-31,26,-31,24,-31,21,-31,22,-31,18,-31,17,-31,16,-31,37,-31,34,-31});
    states[66] = new State(new int[]{27,22,28,44,25,-25,23,-25,26,-25,24,-25,21,-25,22,-25,18,-25,17,-25,16,-25,37,-25,34,-25});
    states[67] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-9,68,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[68] = new State(new int[]{25,20,23,42,26,58,24,60,21,62,22,64,18,-34,17,-34,16,-34,37,-34,34,-34});
    states[69] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-11,70,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[70] = new State(-36);
    states[71] = new State(-42);
    states[72] = new State(new int[]{33,73});
    states[73] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-11,74,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[74] = new State(new int[]{34,75});
    states[75] = new State(new int[]{35,11,38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54,5,72,7,79,8,84,9,88,10,93},new int[]{-12,76,-18,10,-11,15,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50,-15,71,-16,87});
    states[76] = new State(new int[]{6,77,36,-57,35,-57,38,-57,14,-57,15,-57,39,-57,40,-57,33,-57,28,-57,32,-57,31,-57,5,-57,7,-57,8,-57,9,-57,10,-57});
    states[77] = new State(new int[]{35,11,38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54,5,72,7,79,8,84,9,88,10,93},new int[]{-12,78,-18,10,-11,15,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50,-15,71,-16,87});
    states[78] = new State(-58);
    states[79] = new State(new int[]{33,80});
    states[80] = new State(new int[]{38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54},new int[]{-11,81,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50});
    states[81] = new State(new int[]{34,82});
    states[82] = new State(new int[]{35,11,38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54,5,72,7,79,8,84,9,88,10,93},new int[]{-12,83,-18,10,-11,15,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50,-15,71,-16,87});
    states[83] = new State(-43);
    states[84] = new State(new int[]{38,85});
    states[85] = new State(new int[]{37,86});
    states[86] = new State(-44);
    states[87] = new State(-45);
    states[88] = new State(new int[]{38,89,41,91});
    states[89] = new State(new int[]{37,90});
    states[90] = new State(-59);
    states[91] = new State(new int[]{37,92});
    states[92] = new State(-60);
    states[93] = new State(new int[]{37,94});
    states[94] = new State(-46);
    states[95] = new State(-55);
    states[96] = new State(new int[]{36,97,35,11,38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54,5,72,7,79,8,84,9,88,10,93,11,101,12,104,13,107},new int[]{-14,98,-2,100,-12,95,-18,10,-11,15,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50,-15,71,-16,87});
    states[97] = new State(-51);
    states[98] = new State(new int[]{36,99,35,11,38,29,14,30,15,31,39,32,40,33,33,34,28,52,32,53,31,54,5,72,7,79,8,84,9,88,10,93},new int[]{-12,9,-18,10,-11,15,-10,17,-9,41,-8,66,-7,57,-6,56,-4,55,-3,28,-5,50,-15,71,-16,87});
    states[99] = new State(-52);
    states[100] = new State(-54);
    states[101] = new State(new int[]{38,102});
    states[102] = new State(new int[]{37,103});
    states[103] = new State(-37);
    states[104] = new State(new int[]{38,105});
    states[105] = new State(new int[]{37,106});
    states[106] = new State(-38);
    states[107] = new State(new int[]{38,108});
    states[108] = new State(new int[]{37,109});
    states[109] = new State(-39);
    states[110] = new State(-53);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-19, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{4,-17});
    rules[3] = new Rule(-3, new int[]{38});
    rules[4] = new Rule(-3, new int[]{14});
    rules[5] = new Rule(-3, new int[]{15});
    rules[6] = new Rule(-3, new int[]{39});
    rules[7] = new Rule(-3, new int[]{40});
    rules[8] = new Rule(-3, new int[]{33,-11,34});
    rules[9] = new Rule(-4, new int[]{-3});
    rules[10] = new Rule(-4, new int[]{-5,-4});
    rules[11] = new Rule(-5, new int[]{28});
    rules[12] = new Rule(-5, new int[]{32});
    rules[13] = new Rule(-5, new int[]{31});
    rules[14] = new Rule(-5, new int[]{33,12,34});
    rules[15] = new Rule(-5, new int[]{33,11,34});
    rules[16] = new Rule(-6, new int[]{-4});
    rules[17] = new Rule(-6, new int[]{-6,19,-4});
    rules[18] = new Rule(-6, new int[]{-6,20,-4});
    rules[19] = new Rule(-7, new int[]{-6});
    rules[20] = new Rule(-7, new int[]{-7,29,-6});
    rules[21] = new Rule(-7, new int[]{-7,30,-6});
    rules[22] = new Rule(-8, new int[]{-7});
    rules[23] = new Rule(-8, new int[]{-8,27,-7});
    rules[24] = new Rule(-8, new int[]{-8,28,-7});
    rules[25] = new Rule(-9, new int[]{-8});
    rules[26] = new Rule(-9, new int[]{-9,25,-8});
    rules[27] = new Rule(-9, new int[]{-9,23,-8});
    rules[28] = new Rule(-9, new int[]{-9,26,-8});
    rules[29] = new Rule(-9, new int[]{-9,24,-8});
    rules[30] = new Rule(-9, new int[]{-9,21,-8});
    rules[31] = new Rule(-9, new int[]{-9,22,-8});
    rules[32] = new Rule(-10, new int[]{-9});
    rules[33] = new Rule(-10, new int[]{-10,18,-9});
    rules[34] = new Rule(-10, new int[]{-10,17,-9});
    rules[35] = new Rule(-11, new int[]{-10});
    rules[36] = new Rule(-11, new int[]{-10,16,-11});
    rules[37] = new Rule(-2, new int[]{11,38,37});
    rules[38] = new Rule(-2, new int[]{12,38,37});
    rules[39] = new Rule(-2, new int[]{13,38,37});
    rules[40] = new Rule(-12, new int[]{-18});
    rules[41] = new Rule(-12, new int[]{-11,37});
    rules[42] = new Rule(-12, new int[]{-15});
    rules[43] = new Rule(-12, new int[]{7,33,-11,34,-12});
    rules[44] = new Rule(-12, new int[]{8,38,37});
    rules[45] = new Rule(-12, new int[]{-16});
    rules[46] = new Rule(-12, new int[]{10,37});
    rules[47] = new Rule(-18, new int[]{35,36});
    rules[48] = new Rule(-18, new int[]{35,-14,36});
    rules[49] = new Rule(-17, new int[]{35,36});
    rules[50] = new Rule(-17, new int[]{35,-14,36});
    rules[51] = new Rule(-17, new int[]{35,-13,36});
    rules[52] = new Rule(-17, new int[]{35,-13,-14,36});
    rules[53] = new Rule(-13, new int[]{-2});
    rules[54] = new Rule(-13, new int[]{-13,-2});
    rules[55] = new Rule(-14, new int[]{-12});
    rules[56] = new Rule(-14, new int[]{-14,-12});
    rules[57] = new Rule(-15, new int[]{5,33,-11,34,-12});
    rules[58] = new Rule(-15, new int[]{5,33,-11,34,-12,6,-12});
    rules[59] = new Rule(-16, new int[]{9,38,37});
    rules[60] = new Rule(-16, new int[]{9,41,37});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
#pragma warning disable 162, 1522
    switch (action)
    {
      case 2: // program -> Program, main_statement
#line 24 "..\..\parser.y"
  {
			Compiler.tree = new Program(LocationStack[LocationStack.Depth-2].StartLine, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 3: // primary_expression -> Ident
#line 33 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new PrimaryExp(LocationStack[LocationStack.Depth-1].StartLine, PrimaryExpType.Ident, ValueStack[ValueStack.Depth-1].val);
		}
#line default
        break;
      case 4: // primary_expression -> False
#line 37 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new PrimaryExp(LocationStack[LocationStack.Depth-1].StartLine, PrimaryExpType.False, "false");
		}
#line default
        break;
      case 5: // primary_expression -> True
#line 41 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new PrimaryExp(LocationStack[LocationStack.Depth-1].StartLine, PrimaryExpType.True, "true");
		}
#line default
        break;
      case 6: // primary_expression -> RealNumber
#line 45 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new PrimaryExp(LocationStack[LocationStack.Depth-1].StartLine, PrimaryExpType.RealNumber, ValueStack[ValueStack.Depth-1].val);
		}
#line default
        break;
      case 7: // primary_expression -> IntNumber
#line 49 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new PrimaryExp(LocationStack[LocationStack.Depth-1].StartLine, PrimaryExpType.IntNumber, ValueStack[ValueStack.Depth-1].val);
		}
#line default
        break;
      case 8: // primary_expression -> OpenPar, expression, ClosePar
#line 53 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new PrimaryExp(LocationStack[LocationStack.Depth-3].StartLine, PrimaryExpType.Exp, ValueStack[ValueStack.Depth-2].node);
		}
#line default
        break;
      case 9: // unary_expression -> primary_expression
#line 60 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new UnaryExp(LocationStack[LocationStack.Depth-1].StartLine, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 10: // unary_expression -> unary_operator, unary_expression
#line 64 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new UnaryExp(LocationStack[LocationStack.Depth-2].StartLine, ValueStack[ValueStack.Depth-2].node, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 11: // unary_operator -> Minus
#line 71 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new UnaryOp(LocationStack[LocationStack.Depth-1].StartLine, UnaryOpType.Minus);
		}
#line default
        break;
      case 12: // unary_operator -> BitwiseNot
#line 75 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new UnaryOp(LocationStack[LocationStack.Depth-1].StartLine, UnaryOpType.BitwiseNot);
		}
#line default
        break;
      case 13: // unary_operator -> Not
#line 79 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new UnaryOp(LocationStack[LocationStack.Depth-1].StartLine, UnaryOpType.Not);
		}
#line default
        break;
      case 14: // unary_operator -> OpenPar, Double, ClosePar
#line 83 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new UnaryOp(LocationStack[LocationStack.Depth-3].StartLine, UnaryOpType.Cast2Double);
		}
#line default
        break;
      case 15: // unary_operator -> OpenPar, Int, ClosePar
#line 87 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new UnaryOp(LocationStack[LocationStack.Depth-3].StartLine, UnaryOpType.Cast2Int);
		}
#line default
        break;
      case 16: // bitwise_expression -> unary_expression
#line 94 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new BitwiseExp(LocationStack[LocationStack.Depth-1].StartLine, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 17: // bitwise_expression -> bitwise_expression, BitwiseOr, unary_expression
#line 98 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new BitwiseExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, BitwiseOpType.Or);
		}
#line default
        break;
      case 18: // bitwise_expression -> bitwise_expression, BitwiseAnd, unary_expression
#line 102 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new BitwiseExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, BitwiseOpType.And);
		}
#line default
        break;
      case 19: // multiplicative_expression -> bitwise_expression
#line 109 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new MulExp(LocationStack[LocationStack.Depth-1].StartLine, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 20: // multiplicative_expression -> multiplicative_expression, Mul, bitwise_expression
#line 113 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new MulExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, MulOpType.Mul);
		}
#line default
        break;
      case 21: // multiplicative_expression -> multiplicative_expression, Div, bitwise_expression
#line 117 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new MulExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, MulOpType.Div);
		}
#line default
        break;
      case 22: // additive_expression -> multiplicative_expression
#line 124 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new AddExp(LocationStack[LocationStack.Depth-1].StartLine, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 23: // additive_expression -> additive_expression, Plus, multiplicative_expression
#line 128 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new AddExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, AddOpType.Add);
		}
#line default
        break;
      case 24: // additive_expression -> additive_expression, Minus, multiplicative_expression
#line 132 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new AddExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, AddOpType.Sub);
		}
#line default
        break;
      case 25: // relational_expression -> additive_expression
#line 139 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new RelExp(LocationStack[LocationStack.Depth-1].StartLine, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 26: // relational_expression -> relational_expression, LessThan, additive_expression
#line 143 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new RelExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, RelOpType.LessThan);
		}
#line default
        break;
      case 27: // relational_expression -> relational_expression, GreaterThan, 
               //                          additive_expression
#line 147 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new RelExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, RelOpType.GreaterThan);
		}
#line default
        break;
      case 28: // relational_expression -> relational_expression, LessEqual, additive_expression
#line 151 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new RelExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, RelOpType.LessEqual);
		}
#line default
        break;
      case 29: // relational_expression -> relational_expression, GreaterEqual, 
               //                          additive_expression
#line 155 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new RelExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, RelOpType.GreaterEqual);
		}
#line default
        break;
      case 30: // relational_expression -> relational_expression, Equals, additive_expression
#line 159 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new RelExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, RelOpType.Equals);
		}
#line default
        break;
      case 31: // relational_expression -> relational_expression, NotEquals, additive_expression
#line 163 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new RelExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, RelOpType.NotEquals);
		}
#line default
        break;
      case 32: // logical_expression -> relational_expression
#line 170 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new LogExp(LocationStack[LocationStack.Depth-1].StartLine, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 33: // logical_expression -> logical_expression, And, relational_expression
#line 174 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new LogExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, LogOpType.And);
		}
#line default
        break;
      case 34: // logical_expression -> logical_expression, Or, relational_expression
#line 178 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new LogExp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node, LogOpType.Or);
		}
#line default
        break;
      case 35: // expression -> logical_expression
#line 185 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Exp(LocationStack[LocationStack.Depth-1].StartLine, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 36: // expression -> logical_expression, Assign, expression
#line 189 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Exp(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 37: // declaration -> Int, Ident, Semicolon
#line 198 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Decl(LocationStack[LocationStack.Depth-3].StartLine, VarType.Int, ValueStack[ValueStack.Depth-2].val);
		}
#line default
        break;
      case 38: // declaration -> Double, Ident, Semicolon
#line 202 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Decl(LocationStack[LocationStack.Depth-3].StartLine, VarType.Double, ValueStack[ValueStack.Depth-2].val);
		}
#line default
        break;
      case 39: // declaration -> Bool, Ident, Semicolon
#line 206 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Decl(LocationStack[LocationStack.Depth-3].StartLine, VarType.Bool, ValueStack[ValueStack.Depth-2].val);
		}
#line default
        break;
      case 40: // statement -> block_statement
#line 215 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Stat(LocationStack[LocationStack.Depth-1].StartLine, StatType.Block, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 41: // statement -> expression, Semicolon
#line 219 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Stat(LocationStack[LocationStack.Depth-2].StartLine, StatType.Exp, ValueStack[ValueStack.Depth-2].node);
		}
#line default
        break;
      case 42: // statement -> selection_statement
#line 223 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Stat(LocationStack[LocationStack.Depth-1].StartLine, StatType.Selection, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 43: // statement -> While, OpenPar, expression, ClosePar, statement
#line 227 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Stat(LocationStack[LocationStack.Depth-5].StartLine, StatType.While, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 44: // statement -> Read, Ident, Semicolon
#line 231 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Stat(LocationStack[LocationStack.Depth-3].StartLine, StatType.Read, ValueStack[ValueStack.Depth-2].val);
		}
#line default
        break;
      case 45: // statement -> write_statement
#line 235 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Stat(LocationStack[LocationStack.Depth-1].StartLine, StatType.Write, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 46: // statement -> Return, Semicolon
#line 239 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new Stat(LocationStack[LocationStack.Depth-2].StartLine, StatType.Return);
		}
#line default
        break;
      case 47: // block_statement -> OpenCurly, CloseCurly
#line 246 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new BlockStat(LocationStack[LocationStack.Depth-2].StartLine);
		}
#line default
        break;
      case 48: // block_statement -> OpenCurly, statement_list, CloseCurly
#line 250 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new BlockStat(LocationStack[LocationStack.Depth-3].StartLine, ValueStack[ValueStack.Depth-2].node);
		}
#line default
        break;
      case 49: // main_statement -> OpenCurly, CloseCurly
#line 257 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new MainStat(LocationStack[LocationStack.Depth-2].StartLine, MainStatType.Empty);
		}
#line default
        break;
      case 50: // main_statement -> OpenCurly, statement_list, CloseCurly
#line 261 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new MainStat(LocationStack[LocationStack.Depth-3].StartLine, MainStatType.Stat, ValueStack[ValueStack.Depth-2].node);
		}
#line default
        break;
      case 51: // main_statement -> OpenCurly, declaration_list, CloseCurly
#line 265 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new MainStat(LocationStack[LocationStack.Depth-3].StartLine, MainStatType.Decl, ValueStack[ValueStack.Depth-2].node);
		}
#line default
        break;
      case 52: // main_statement -> OpenCurly, declaration_list, statement_list, CloseCurly
#line 269 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new MainStat(LocationStack[LocationStack.Depth-4].StartLine, MainStatType.DeclStat, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-2].node);
		}
#line default
        break;
      case 53: // declaration_list -> declaration
#line 276 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new DeclList(LocationStack[LocationStack.Depth-1].StartLine, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 54: // declaration_list -> declaration_list, declaration
#line 280 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new DeclList(LocationStack[LocationStack.Depth-2].StartLine, ValueStack[ValueStack.Depth-2].node, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 55: // statement_list -> statement
#line 287 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new StatList(LocationStack[LocationStack.Depth-1].StartLine, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 56: // statement_list -> statement_list, statement
#line 291 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new StatList(LocationStack[LocationStack.Depth-2].StartLine, ValueStack[ValueStack.Depth-2].node, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 57: // selection_statement -> If, OpenPar, expression, ClosePar, statement
#line 298 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new SelectionStat(LocationStack[LocationStack.Depth-5].StartLine, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 58: // selection_statement -> If, OpenPar, expression, ClosePar, statement, Else, 
               //                        statement
#line 302 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new SelectionStat(LocationStack[LocationStack.Depth-7].StartLine, ValueStack[ValueStack.Depth-5].node, ValueStack[ValueStack.Depth-3].node, ValueStack[ValueStack.Depth-1].node);
		}
#line default
        break;
      case 59: // write_statement -> Write, Ident, Semicolon
#line 309 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new WriteStat(LocationStack[LocationStack.Depth-3].StartLine, WriteStatType.Ident, ValueStack[ValueStack.Depth-2].val);
		}
#line default
        break;
      case 60: // write_statement -> Write, String, Semicolon
#line 313 "..\..\parser.y"
  {
			CurrentSemanticValue.node = new WriteStat(LocationStack[LocationStack.Depth-3].StartLine, WriteStatType.String, ValueStack[ValueStack.Depth-2].val);
		}
#line default
        break;
    }
#pragma warning restore 162, 1522
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliases != null && aliases.ContainsKey(terminal))
        return aliases[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

#line 319 "..\..\parser.y"

public Parser(Scanner scanner) : base(scanner) { }
#line default
}
}
