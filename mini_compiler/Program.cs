using System;
using System.IO;
using System.Collections.Generic;
using GardensPoint;

public class Compiler
{

    public static int errors = 0;

    public static List<string> source;

    public static SyntaxTree tree;

    // arg[0] określa plik źródłowy
    // pozostałe argumenty są ignorowane
    public static int Main(string[] args)
    {
        string file;
        FileStream source;
        Console.WriteLine("\nMulti-Pass mini language compiler - Gardens Point");
        if (args.Length >= 1)
            file = args[0];
        else
        {
            Console.Write("\nsource file:  ");
            file = Console.ReadLine();
        }
        try
        {
            var sr = new StreamReader(file);
            string str = sr.ReadToEnd();
            sr.Close();
            Compiler.source = new System.Collections.Generic.List<string>(str.Split(new string[] { "\r\n" }, System.StringSplitOptions.None));
            source = new FileStream(file, FileMode.Open);
        }
        catch (Exception e)
        {
            Console.WriteLine("\n" + e.Message);
            return 1;
        }
        Scanner scanner = new Scanner(source);
        Parser parser = new Parser(scanner);
        Console.WriteLine();
        sw = new StreamWriter(file + ".il");
        GenProlog();
        parser.Parse();
        GenEpilog();
        sw.Close();
        source.Close();
        if (errors == 0)
            Console.WriteLine("  compilation successful\n");
        else
        {
            Console.WriteLine($"\n  {errors} errors detected\n");
            File.Delete(file + ".il");
        }
        return errors == 0 ? 0 : 2;
    }

    public static void EmitCode(string instr = null)
    {
        sw.WriteLine(instr);
    }

    public static void EmitCode(string instr, params object[] args)
    {
        sw.WriteLine(instr, args);
    }

    private static StreamWriter sw;

    private static void GenProlog()
    {
        EmitCode(".assembly extern mscorlib { }");
        EmitCode(".assembly mini_compiler { }");
        EmitCode(".method static void main()");
        EmitCode("{");
        EmitCode(".entrypoint");
        EmitCode(".try");
        EmitCode("{");
        EmitCode();

        EmitCode("-------------------------// prolog");
        EmitCode();
    }

    private static void GenEpilog()
    {
        EmitCode("-------------------------// epilog:");
        EmitCode("leave EndMain");
        EmitCode("}");
        EmitCode("catch [mscorlib]System.Exception");
        EmitCode("{");
        EmitCode("callvirt instance string [mscorlib]System.Exception::get_Message()");
        EmitCode("call void [mscorlib]System.Console::WriteLine(string)");
        EmitCode("leave EndMain");
        EmitCode("}");
        EmitCode("EndMain: ret");
        EmitCode("}");
    }
}
#region Enums
public enum PrimaryExpType
{
    Ident, False, True, RealNumber, IntNumber, Exp
}
public enum UnaryExpType
{
    Empty, UnaryExp
}
public enum UnaryOpType
{
    Plus, Minus, Not, BitwiseNot, Cast2Double, Cast2Int
}
public enum BitwiseOpType
{
    Empty, Or, And
}
public enum MulOpType
{
    Empty, Mul, Div
}
public enum AddOpType
{
    Empty, Add, Sub
}
public enum RelOpType
{
    Empty, LessThan, GreaterThan, LessEqual, GreaterEqual, Equals, NotEquals
}
public enum LogOpType
{
    Empty, And, Or
}
public enum ExpOpType
{
    Empty, Assign
}
public enum VarType
{
    Int, Double, Bool
}
public enum StatType
{
    Cmp, Exp, Selection, While, Read, Write, Return
}
public enum CmpStatType
{
    Empty, Stat, Decl, DeclStat
}
public enum DeclListType
{
    Decl, DeclList
}
public enum StatListType
{
    Stat, StatList
}
public enum SelectionStatType
{
    If, IfElse
}
public enum WriteStatType
{
    Ident, String
}
#endregion
#region SyntaxTree
public abstract class SyntaxTree
{
    public int _line = -1;
    public List<SyntaxTree> childs = new List<SyntaxTree>();
    public SyntaxTree(int line)
    {
        _line = line;
    }
    public abstract string GenCode();
}
public class Program : SyntaxTree
{
    public Program(int lineNum, SyntaxTree child) : base(lineNum)
    {
        childs.Add(child);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class PrimaryExp : SyntaxTree
{
    public PrimaryExpType _type;
    public string _val = "";
    public PrimaryExp(int lineNum, PrimaryExpType type) : base(lineNum)
    {
        _type = type;
    }
    public PrimaryExp(int lineNum, PrimaryExpType type, string val) : this(lineNum, type)
    {
        _val = val;
    }
    public PrimaryExp(int lineNum, PrimaryExpType type, SyntaxTree child) : this(lineNum, type)
    {
        childs.Add(child);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}

public class UnaryExp : SyntaxTree
{
    public UnaryExpType _type;
    public UnaryExp(int lineNum, SyntaxTree child) : base(lineNum)
    {
        childs.Add(child);
        _type = UnaryExpType.Empty;
    }
    public UnaryExp(int lineNum, SyntaxTree childOp, SyntaxTree childExp) : base(lineNum)
    {
        childs.Add(childOp);
        childs.Add(childExp);
        _type = UnaryExpType.UnaryExp;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class UnaryOp : SyntaxTree
{
    public UnaryOpType _type;
    public UnaryOp(int lineNum, UnaryOpType type) : base(lineNum)
    {
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class BitwiseExp : SyntaxTree
{
    public BitwiseOpType _type;
    public BitwiseExp(int lineNum, SyntaxTree childUnExp) : base(lineNum)
    {
        childs.Add(childUnExp);
        _type = BitwiseOpType.Empty;
    }
    public BitwiseExp(int lineNum, SyntaxTree childBitExp, SyntaxTree childUnExp, BitwiseOpType type) : base(lineNum)
    {
        childs.Add(childBitExp);
        childs.Add(childUnExp);
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class MulExp : SyntaxTree
{
    public MulOpType _type;
    public MulExp(int lineNum, SyntaxTree childBitExp) : base(lineNum)
    {
        childs.Add(childBitExp);
        _type = MulOpType.Empty;
    }
    public MulExp(int lineNum, SyntaxTree childMulExp, SyntaxTree childBitExp, MulOpType type) : base(lineNum)
    {
        childs.Add(childMulExp);
        childs.Add(childBitExp);
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}

public class AddExp : SyntaxTree
{
    public AddOpType _type;
    public AddExp(int lineNum, SyntaxTree childMulExp) : base(lineNum)
    {
        childs.Add(childMulExp);
        _type = AddOpType.Empty;
    }
    public AddExp(int lineNum, SyntaxTree childAddExp, SyntaxTree childMulExp, AddOpType type) : base(lineNum)
    {
        childs.Add(childAddExp);
        childs.Add(childMulExp);
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class RelExp : SyntaxTree
{
    public RelOpType _type;
    public RelExp(int lineNum, SyntaxTree childAddExp) : base(lineNum)
    {
        childs.Add(childAddExp);
        _type = RelOpType.Empty;
    }
    public RelExp(int lineNum, SyntaxTree childRelExp, SyntaxTree childAddExp, RelOpType type) : base(lineNum)
    {
        childs.Add(childRelExp);
        childs.Add(childAddExp);
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class LogExp : SyntaxTree
{
    public LogOpType _type;
    public LogExp(int lineNum, SyntaxTree childRelExp) : base(lineNum)
    {
        childs.Add(childRelExp);
        _type = LogOpType.Empty;
    }
    public LogExp(int lineNum, SyntaxTree childLogExp, SyntaxTree childRelExp, LogOpType type) : base(lineNum)
    {
        childs.Add(childLogExp);
        childs.Add(childRelExp);
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class Exp : SyntaxTree
{
    public ExpOpType _type;
    public Exp(int lineNum, SyntaxTree childLogExp) : base(lineNum)
    {
        childs.Add(childLogExp);
        _type = ExpOpType.Empty;
    }
    public Exp(int lineNum, SyntaxTree childLogExp, SyntaxTree childExp) : base(lineNum)
    {
        childs.Add(childLogExp);
        childs.Add(childExp);
        _type = ExpOpType.Assign;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class Decl : SyntaxTree
{
    public string _val;
    public VarType _type;
    public Decl(int lineNum, VarType type, string val) : base(lineNum)
    {
        _val = val;
        _type = type;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class Stat : SyntaxTree
{
    public StatType _type;
    public string _ident = "";
    public Stat(int lineNum, StatType type) : base(lineNum)
    {
        _type = type;
    }
    public Stat(int lineNum, StatType type, SyntaxTree childStat) : this(lineNum, type)
    {
        childs.Add(childStat);
    }
    public Stat(int lineNum, StatType type, SyntaxTree childExp, SyntaxTree childStat) : this(lineNum, type)
    {
        childs.Add(childExp);
        childs.Add(childStat);
    }
    public Stat(int lineNum, StatType type, string ident) : this(lineNum, type)
    {
        _ident = ident;
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class CmpStat : SyntaxTree
{
    public string _val;
    public CmpStatType _type;
    public CmpStat(int lineNum, CmpStatType type) : base(lineNum)
    {
        _type = type;
    }
    public CmpStat(int lineNum, CmpStatType type, SyntaxTree child) : this(lineNum, type)
    {
        childs.Add(child);
    }
    public CmpStat(int lineNum, CmpStatType type, SyntaxTree childDecl, SyntaxTree childStat) : this(lineNum, type)
    {
        childs.Add(childDecl);
        childs.Add(childStat);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class DeclList : SyntaxTree
{
    public DeclListType _type;
    public DeclList(int lineNum, SyntaxTree childDecl) : base(lineNum)
    {
        _type = DeclListType.Decl;
        childs.Add(childDecl);
    }
    public DeclList(int lineNum, SyntaxTree childDeclList, SyntaxTree childDecl) : base(lineNum)
    {
        _type = DeclListType.DeclList;
        childs.Add(childDeclList);
        childs.Add(childDecl);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class StatList : SyntaxTree
{
    public StatListType _type;
    public StatList(int lineNum, SyntaxTree childStat) : base(lineNum)
    {
        _type = StatListType.Stat;
        childs.Add(childStat);
    }
    public StatList(int lineNum, SyntaxTree childStatList, SyntaxTree childStat) : base(lineNum)
    {
        _type = StatListType.StatList;
        childs.Add(childStatList);
        childs.Add(childStat);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class SelectionStat : SyntaxTree
{
    public SelectionStatType _type;
    public SelectionStat(int lineNum, SyntaxTree childExp, SyntaxTree childStat) : base(lineNum)
    {
        _type = SelectionStatType.If;
        childs.Add(childExp);
        childs.Add(childStat);
    }
    public SelectionStat(int lineNum, SyntaxTree childExp, SyntaxTree childStat1, SyntaxTree childStat2) : base(lineNum)
    {
        _type = SelectionStatType.IfElse;
        childs.Add(childExp);
        childs.Add(childStat1);
        childs.Add(childStat2);
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
public class WriteStat : SyntaxTree
{
    public string _stringVal = "", _ident = "";
    public WriteStatType _type;
    public WriteStat(int lineNum, WriteStatType type, string stringOrIdent) : base(lineNum)
    {
        _type = type;
        switch (_type)
        {
            case WriteStatType.Ident:
                _ident = stringOrIdent;
                break;
            case WriteStatType.String:
                _stringVal = stringOrIdent;
                break;
        }
    }
    public override string GenCode()
    {
        throw new NotImplementedException();
    }
}
#endregion