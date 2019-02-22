// created by jay 0.7 (c) 1998 Axel.Schreiner@informatik.uni-osnabrueck.de

#line 2 "Repil/IR/IR.jay"
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Linq;

using Repil.Types;

#pragma warning disable 219,414

namespace Repil.IR
{
	public partial class Parser
	{
#line default

  /** error output stream.
      It should be changeable.
    */
  public System.IO.TextWriter ErrorOutput = new StringWriter ();

  /** simplified error message.
      @see <a href="#yyerror(java.lang.String, java.lang.String[])">yyerror</a>
    */
  public void yyerror (string message) {
    yyerror(message, null);
  }

  /* An EOF token */
  public int eof_token;
  
  public int yacc_verbose_flag;

  /** (syntax) error message.
      Can be overwritten to control message format.
      @param message text to be displayed.
      @param expected vector of acceptable tokens, if available.
    */
  public void yyerror (string message, string[] expected) {
    if ((yacc_verbose_flag > 0) && (expected != null) && (expected.Length  > 0)) {
      ErrorOutput.Write (message+", expecting");
      for (int n = 0; n < expected.Length; ++ n)
        ErrorOutput.Write (" "+expected[n]);
        ErrorOutput.WriteLine ();
    } else
      ErrorOutput.WriteLine (message);
  }

  /** debugging support, requires the package jay.yydebug.
      Set to null to suppress debugging messages.
    */
//t  internal yydebug.yyDebug debug;

  protected const int yyFinal = 9;
//t // Put this array into a separate class so it is only initialized if debugging is actually used
//t // Use MarshalByRefObject to disable inlining
//t class YYRules : MarshalByRefObject {
//t  public static readonly string [] yyRule = {
//t    "$accept : module",
//t    "module : module_parts",
//t    "module_parts : module_part",
//t    "module_parts : module_parts module_part",
//t    "module_part : SOURCE_FILENAME '=' STRING",
//t    "module_part : TARGET DATALAYOUT '=' STRING",
//t    "module_part : TARGET TRIPLE '=' STRING",
//t    "module_part : LOCAL_SYMBOL '=' TYPE literal_structure",
//t    "module_part : LOCAL_SYMBOL '=' TYPE OPAQUE",
//t    "module_part : function_definition",
//t    "module_part : function_declaration",
//t    "module_part : global_variable",
//t    "module_part : ATTRIBUTES ATTRIBUTE_GROUP_REF '=' '{' attributes '}'",
//t    "module_part : META_SYMBOL_DEF '=' '!' '{' '}'",
//t    "module_part : META_SYMBOL_DEF '=' '!' '{' metadata '}'",
//t    "module_part : META_SYMBOL_DEF '=' META_SYMBOL '(' metadata_args ')'",
//t    "module_part : META_SYMBOL_DEF '=' DISTINCT '!' '{' metadata '}'",
//t    "module_part : META_SYMBOL_DEF '=' DISTINCT META_SYMBOL '(' metadata_args ')'",
//t    "global_variable : GLOBAL_SYMBOL '=' global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' function_addr global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility function_addr global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility function_addr global_kind type value",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility function_addr global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_kind : GLOBAL",
//t    "global_kind : CONSTANT",
//t    "linkage : EXTERNAL",
//t    "linkage : INTERNAL",
//t    "visibility : PRIVATE",
//t    "metadata_args : metadata_arg",
//t    "metadata_args : metadata_args ',' metadata_arg",
//t    "metadata_arg : SYMBOL ':' SYMBOL",
//t    "metadata_arg : SYMBOL ':' META_SYMBOL",
//t    "metadata_arg : SYMBOL ':' STRING",
//t    "metadata_arg : SYMBOL ':' constant",
//t    "metadata_arg : TYPE ':' META_SYMBOL",
//t    "metadata_arg : SYMBOL ':' META_SYMBOL '(' metadata_value_args ')'",
//t    "metadata_arg : SYMBOL ':' META_SYMBOL '(' ')'",
//t    "metadata_kvs : META_SYMBOL META_SYMBOL",
//t    "metadata_kvs : metadata_kvs META_SYMBOL META_SYMBOL",
//t    "metadata : metadatum",
//t    "metadata : metadata META_SYMBOL",
//t    "metadata : metadata ',' typed_value",
//t    "metadata : metadata ',' META_SYMBOL",
//t    "metadata : metadata ',' NULL",
//t    "metadatum : typed_value",
//t    "metadatum : META_SYMBOL",
//t    "metadatum : NULL",
//t    "attributes : attribute",
//t    "attributes : attributes attribute",
//t    "attribute : NORECURSE",
//t    "attribute : NOUNWIND",
//t    "attribute : READNONE",
//t    "attribute : SPECULATABLE",
//t    "attribute : SSP",
//t    "attribute : UWTABLE",
//t    "attribute : ARGMEMONLY",
//t    "attribute : STRING '=' STRING",
//t    "attribute : STRING",
//t    "attribute : SYMBOL",
//t    "attribute : READONLY",
//t    "attribute : SYMBOL '(' metadata_value_args ')'",
//t    "literal_structure : '{' '}'",
//t    "literal_structure : '{' type_list '}'",
//t    "literal_structure : '<' '{' type_list '}' '>'",
//t    "type_list : type",
//t    "type_list : type_list ',' type",
//t    "return_type : type",
//t    "return_type : VOID",
//t    "type : literal_structure",
//t    "type : INTEGER_TYPE",
//t    "type : HALF",
//t    "type : FLOAT",
//t    "type : DOUBLE",
//t    "type : X86_FP80",
//t    "type : return_type '(' ')'",
//t    "type : return_type '(' function_type_args ')'",
//t    "type : type '*'",
//t    "type : LOCAL_SYMBOL",
//t    "type : '<' INTEGER X type '>'",
//t    "type : '[' INTEGER X type ']'",
//t    "function_type_args : function_type_arg",
//t    "function_type_args : function_type_args ',' function_type_arg",
//t    "function_type_arg : type",
//t    "function_type_arg : ELLIPSIS",
//t    "function_definition : DEFINE return_type GLOBAL_SYMBOL parameters function_addr attribute_group_refs '{' blocks '}'",
//t    "function_definition : DEFINE return_type GLOBAL_SYMBOL parameters attribute_group_refs metadata_kvs '{' blocks '}'",
//t    "function_definition : DEFINE return_type GLOBAL_SYMBOL parameters function_addr attribute_group_refs metadata_kvs '{' blocks '}'",
//t    "function_definition : DEFINE NOALIAS return_type GLOBAL_SYMBOL parameters function_addr attribute_group_refs '{' blocks '}'",
//t    "function_definition : DEFINE NOALIAS return_type GLOBAL_SYMBOL parameters function_addr attribute_group_refs metadata_kvs '{' blocks '}'",
//t    "function_definition : DEFINE linkage return_type GLOBAL_SYMBOL parameters attribute_group_refs '{' blocks '}'",
//t    "function_definition : DEFINE linkage return_type GLOBAL_SYMBOL parameters attribute_group_refs metadata_kvs '{' blocks '}'",
//t    "function_definition : DEFINE linkage return_type GLOBAL_SYMBOL parameters function_addr attribute_group_refs metadata_kvs '{' blocks '}'",
//t    "function_definition : DEFINE linkage calling_convention return_type GLOBAL_SYMBOL parameters function_addr attribute_group_refs '{' blocks '}'",
//t    "function_definition : DEFINE linkage calling_convention parameter_attribute return_type GLOBAL_SYMBOL parameters function_addr attribute_group_refs metadata_kvs '{' blocks '}'",
//t    "function_definition : DEFINE linkage calling_convention return_type GLOBAL_SYMBOL parameters function_addr attribute_group_refs metadata_kvs '{' blocks '}'",
//t    "function_declaration : DECLARE return_type GLOBAL_SYMBOL parameters attribute_group_refs",
//t    "function_declaration : DECLARE NOALIAS return_type GLOBAL_SYMBOL parameters attribute_group_refs",
//t    "function_declaration : DECLARE return_type GLOBAL_SYMBOL parameters function_addr attribute_group_refs",
//t    "parameters : '(' parameter_list ')'",
//t    "parameters : '(' ')'",
//t    "parameter_list : parameter",
//t    "parameter_list : parameter_list ',' parameter",
//t    "parameter : type",
//t    "parameter : type parameter_attributes",
//t    "parameter : METADATA",
//t    "parameter : ELLIPSIS",
//t    "parameter_attributes : parameter_attribute",
//t    "parameter_attributes : parameter_attributes parameter_attribute",
//t    "parameter_attribute : NONNULL",
//t    "parameter_attribute : NOCAPTURE",
//t    "parameter_attribute : READONLY",
//t    "parameter_attribute : WRITEONLY",
//t    "parameter_attribute : READNONE",
//t    "parameter_attribute : SIGNEXT",
//t    "parameter_attribute : ZEROEXT",
//t    "parameter_attribute : RETURNED",
//t    "function_addr : UNNAMED_ADDR",
//t    "function_addr : LOCAL_UNNAMED_ADDR",
//t    "attribute_group_refs : attribute_group_ref",
//t    "attribute_group_refs : attribute_group_refs attribute_group_ref",
//t    "attribute_group_ref : ATTRIBUTE_GROUP_REF",
//t    "icmp_condition : EQ",
//t    "icmp_condition : NE",
//t    "icmp_condition : UGT",
//t    "icmp_condition : UGE",
//t    "icmp_condition : ULT",
//t    "icmp_condition : ULE",
//t    "icmp_condition : SGT",
//t    "icmp_condition : SGE",
//t    "icmp_condition : SLT",
//t    "icmp_condition : SLE",
//t    "fcmp_condition : TRUE",
//t    "fcmp_condition : FALSE",
//t    "fcmp_condition : ORD",
//t    "fcmp_condition : OEQ",
//t    "fcmp_condition : ONE",
//t    "fcmp_condition : OGT",
//t    "fcmp_condition : OGE",
//t    "fcmp_condition : OLT",
//t    "fcmp_condition : OLE",
//t    "fcmp_condition : UNO",
//t    "fcmp_condition : UEQ",
//t    "fcmp_condition : UNE",
//t    "fcmp_condition : UGT",
//t    "fcmp_condition : UGE",
//t    "fcmp_condition : ULT",
//t    "fcmp_condition : ULE",
//t    "value : constant",
//t    "value : LOCAL_SYMBOL",
//t    "value : GLOBAL_SYMBOL",
//t    "value : INTTOPTR '(' typed_value TO type ')'",
//t    "value : GETELEMENTPTR INBOUNDS '(' type ',' typed_value ',' element_indices ')'",
//t    "value : BITCAST '(' typed_value TO type ')'",
//t    "value : PTRTOINT '(' typed_value TO type ')'",
//t    "value : '<' typed_values '>'",
//t    "value : '[' typed_values ']'",
//t    "value : '{' typed_values '}'",
//t    "pointer_value : value",
//t    "constant : NULL",
//t    "constant : FLOAT_LITERAL",
//t    "constant : INTEGER",
//t    "constant : HEX_INTEGER",
//t    "constant : TRUE",
//t    "constant : FALSE",
//t    "constant : UNDEF",
//t    "constant : ZEROINITIALIZER",
//t    "constant : CONSTANT_BYTES",
//t    "label_value : LABEL LOCAL_SYMBOL",
//t    "typed_value : type value",
//t    "typed_value : VOID",
//t    "typed_pointer_value : type pointer_value",
//t    "typed_values : typed_value",
//t    "typed_values : typed_values ',' typed_value",
//t    "typed_constant : type constant",
//t    "element_index : typed_value",
//t    "element_indices : element_index",
//t    "element_indices : element_indices ',' element_index",
//t    "blocks : block",
//t    "blocks : blocks block",
//t    "block : assignments terminator_instruction",
//t    "block : assignments terminator_instruction metadata_kvs",
//t    "block : terminator_instruction",
//t    "block : terminator_instruction metadata_kvs",
//t    "assignments : assignment",
//t    "assignments : assignments assignment",
//t    "assignment : instruction",
//t    "assignment : instruction metadata_kvs",
//t    "assignment : LOCAL_SYMBOL '=' instruction",
//t    "assignment : LOCAL_SYMBOL '=' instruction metadata_kvs",
//t    "function_pointer : value",
//t    "function_args : '(' function_arg_list ')'",
//t    "function_args : '(' ')'",
//t    "function_arg_list : function_arg",
//t    "function_arg_list : function_arg_list ',' function_arg",
//t    "function_arg : type value",
//t    "function_arg : type parameter_attributes value",
//t    "function_arg : METADATA type LOCAL_SYMBOL",
//t    "function_arg : METADATA type metadata_value",
//t    "function_arg : METADATA META_SYMBOL",
//t    "function_arg : METADATA META_SYMBOL '(' ')'",
//t    "function_arg : METADATA META_SYMBOL '(' metadata_value_args ')'",
//t    "metadata_value : constant",
//t    "metadata_value : GLOBAL_SYMBOL",
//t    "metadata_value_args : metadata_value_arg",
//t    "metadata_value_args : metadata_value_args ',' metadata_value_arg",
//t    "metadata_value_arg : constant",
//t    "metadata_value_arg : SYMBOL",
//t    "phi_vals : phi_val",
//t    "phi_vals : phi_vals ',' phi_val",
//t    "phi_val : '[' value ',' value ']'",
//t    "switch_cases : switch_case",
//t    "switch_cases : switch_cases switch_case",
//t    "switch_case : typed_constant ',' label_value",
//t    "wrappings : wrapping",
//t    "wrappings : wrappings wrapping",
//t    "wrapping : NUW",
//t    "wrapping : NSW",
//t    "calling_convention : FASTCC",
//t    "atomic_constraint : SEQ_CST",
//t    "terminator_instruction : BR label_value",
//t    "terminator_instruction : BR INTEGER_TYPE value ',' label_value ',' label_value",
//t    "terminator_instruction : RET typed_value",
//t    "terminator_instruction : SWITCH typed_value ',' label_value '[' switch_cases ']'",
//t    "instruction : ADD type value ',' value",
//t    "instruction : ADD wrappings type value ',' value",
//t    "instruction : ALLOCA type ',' ALIGN INTEGER",
//t    "instruction : AND type value ',' value",
//t    "instruction : ASHR type value ',' value",
//t    "instruction : ASHR EXACT type value ',' value",
//t    "instruction : BITCAST typed_value TO type",
//t    "instruction : CALL return_type function_pointer function_args",
//t    "instruction : CALL calling_convention return_type function_pointer function_args",
//t    "instruction : CALL calling_convention parameter_attribute return_type function_pointer function_args",
//t    "instruction : CALL return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL return_type function_pointer function_args",
//t    "instruction : TAIL CALL calling_convention return_type function_pointer function_args",
//t    "instruction : TAIL CALL calling_convention parameter_attribute return_type function_pointer function_args",
//t    "instruction : EXTRACTELEMENT typed_value ',' typed_value",
//t    "instruction : FADD type value ',' value",
//t    "instruction : FCMP fcmp_condition type value ',' value",
//t    "instruction : FDIV type value ',' value",
//t    "instruction : FENCE atomic_constraint",
//t    "instruction : FMUL type value ',' value",
//t    "instruction : FPEXT typed_value TO type",
//t    "instruction : FPTOUI typed_value TO type",
//t    "instruction : FPTOSI typed_value TO type",
//t    "instruction : FPTRUNC typed_value TO type",
//t    "instruction : FSUB type value ',' value",
//t    "instruction : GETELEMENTPTR type ',' typed_value ',' element_indices",
//t    "instruction : GETELEMENTPTR INBOUNDS type ',' typed_value ',' element_indices",
//t    "instruction : ICMP icmp_condition type value ',' value",
//t    "instruction : INSERTELEMENT typed_value ',' typed_value ',' typed_value",
//t    "instruction : INTTOPTR typed_value TO type",
//t    "instruction : LOAD type ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : LOAD VOLATILE type ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : LSHR type value ',' value",
//t    "instruction : LSHR EXACT type value ',' value",
//t    "instruction : OR type value ',' value",
//t    "instruction : MUL type value ',' value",
//t    "instruction : MUL wrappings type value ',' value",
//t    "instruction : PHI type phi_vals",
//t    "instruction : PTRTOINT typed_value TO type",
//t    "instruction : SDIV type value ',' value",
//t    "instruction : SELECT type value ',' typed_value ',' typed_value",
//t    "instruction : SEXT typed_value TO type",
//t    "instruction : SHL type value ',' value",
//t    "instruction : SHL wrappings type value ',' value",
//t    "instruction : SHUFFLEVECTOR typed_value ',' typed_value ',' typed_value",
//t    "instruction : SITOFP typed_value TO type",
//t    "instruction : SREM type value ',' value",
//t    "instruction : STORE typed_value ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : STORE VOLATILE typed_value ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : SUB type value ',' value",
//t    "instruction : SUB wrappings type value ',' value",
//t    "instruction : TRUNC typed_value TO type",
//t    "instruction : UDIV type value ',' value",
//t    "instruction : UITOFP typed_value TO type",
//t    "instruction : UREM type value ',' value",
//t    "instruction : XOR type value ',' value",
//t    "instruction : ZEXT typed_value TO type",
//t  };
//t public static string getRule (int index) {
//t    return yyRule [index];
//t }
//t}
  protected static readonly string [] yyNames = {    
    "end-of-file",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,"'!'",null,null,null,null,null,
    null,"'('","')'","'*'",null,"','",null,null,null,null,null,null,null,
    null,null,null,null,null,null,"':'",null,"'<'","'='","'>'",null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,"'['",
    null,"']'",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,"'{'",null,"'}'",null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,
    "INTEGER","HEX_INTEGER","FLOAT_LITERAL","STRING","TRUE","FALSE",
    "UNDEF","VOID","NULL","LABEL","X","SOURCE_FILENAME","TARGET",
    "DATALAYOUT","TRIPLE","GLOBAL_SYMBOL","LOCAL_SYMBOL","META_SYMBOL",
    "META_SYMBOL_DEF","SYMBOL","DISTINCT","METADATA","CONSTANT_BYTES",
    "TYPE","HALF","FLOAT","DOUBLE","X86_FP80","INTEGER_TYPE",
    "ZEROINITIALIZER","OPAQUE","DEFINE","DECLARE","UNNAMED_ADDR",
    "LOCAL_UNNAMED_ADDR","NOALIAS","ELLIPSIS","GLOBAL","CONSTANT",
    "PRIVATE","INTERNAL","EXTERNAL","FASTCC","SIGNEXT","ZEROEXT",
    "VOLATILE","RETURNED","NONNULL","NOCAPTURE","WRITEONLY","READONLY",
    "READNONE","ATTRIBUTE_GROUP_REF","ATTRIBUTES","NORECURSE","NOUNWIND",
    "SPECULATABLE","SSP","UWTABLE","ARGMEMONLY","SEQ_CST","RET","BR",
    "SWITCH","INDIRECTBR","INVOKE","RESUME","CATCHSWITCH","CATCHRET",
    "CLEANUPRET","UNREACHABLE","FNEG","ADD","NUW","NSW","FADD","SUB",
    "FSUB","MUL","FMUL","UDIV","SDIV","FDIV","UREM","SREM","FREM","SHL",
    "LSHR","EXACT","ASHR","AND","OR","XOR","EXTRACTELEMENT",
    "INSERTELEMENT","SHUFFLEVECTOR","EXTRACTVALUE","INSERTVALUE","ALLOCA",
    "LOAD","STORE","FENCE","CMPXCHG","ATOMICRMW","GETELEMENTPTR","ALIGN",
    "INBOUNDS","INRANGE","TRUNC","ZEXT","SEXT","FPTRUNC","FPEXT","TO",
    "FPTOUI","FPTOSI","UITOFP","SITOFP","PTRTOINT","INTTOPTR","BITCAST",
    "ADDRSPACECAST","ICMP","EQ","NE","UGT","UGE","ULT","ULE","SGT","SGE",
    "SLT","SLE","FCMP","OEQ","OGT","OGE","OLT","OLE","ONE","ORD","UEQ",
    "UNE","UNO","PHI","SELECT","CALL","TAIL","VA_ARG","LANDINGPAD",
    "CATCHPAD","CLEANUPPAD",
  };

  /** index-checked interface to yyNames[].
      @param token single character or %token value.
      @return token name or [illegal] or [unknown].
    */
  public static string yyname (int token) {
    if ((token < 0) || (token > yyNames.Length)) return "[illegal]";
    string name;
    if ((name = yyNames[token]) != null) return name;
    return "[unknown]";
  }

  //int yyExpectingState;
  /** computes list of expected tokens on error by tracing the tables.
      @param state for which to compute the list.
      @return list of token names.
    */
  protected int [] yyExpectingTokens (int state){
    int token, n, len = 0;
    bool[] ok = new bool[yyNames.Length];
    if ((n = yySindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    if ((n = yyRindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    int [] result = new int [len];
    for (n = token = 0; n < len;  ++ token)
      if (ok[token]) result[n++] = token;
    return result;
  }
  protected string[] yyExpecting (int state) {
    int [] tokens = yyExpectingTokens (state);
    string [] result = new string[tokens.Length];
    for (int n = 0; n < tokens.Length;  n++)
      result[n] = yyNames[tokens [n]];
    return result;
  }

  /** the generated parser, with debugging messages.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @param yydebug debug message writer implementing yyDebug, or null.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex, Object yyd)
				 {
//t    this.debug = (yydebug.yyDebug)yyd;
    return yyparse(yyLex);
  }

  /** initial size and increment of the state/value stack [default 256].
      This is not final so that it can be overwritten outside of invocations
      of yyparse().
    */
  protected int yyMax;

  /** executed at the beginning of a reduce action.
      Used as $$ = yyDefault($1), prior to the user-specified action, if any.
      Can be overwritten to provide deep copy, etc.
      @param first value for $1, or null.
      @return first.
    */
  protected Object yyDefault (Object first) {
    return first;
  }

	static int[] global_yyStates;
	static object[] global_yyVals;
	protected bool use_global_stacks;
	object[] yyVals;					// value stack
	object yyVal;						// value stack ptr
	int yyToken;						// current input
	int yyTop;

  /** the generated parser.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex)
  {
    if (yyMax <= 0) yyMax = 256;		// initial size
    int yyState = 0;                   // state stack ptr
    int [] yyStates;               	// state stack 
    yyVal = null;
    yyToken = -1;
    int yyErrorFlag = 0;				// #tks to shift
	if (use_global_stacks && global_yyStates != null) {
		yyVals = global_yyVals;
		yyStates = global_yyStates;
   } else {
		yyVals = new object [yyMax];
		yyStates = new int [yyMax];
		if (use_global_stacks) {
			global_yyVals = yyVals;
			global_yyStates = yyStates;
		}
	}

    /*yyLoop:*/ for (yyTop = 0;; ++ yyTop) {
      if (yyTop >= yyStates.Length) {			// dynamically increase
        global::System.Array.Resize (ref yyStates, yyStates.Length+yyMax);
        global::System.Array.Resize (ref yyVals, yyVals.Length+yyMax);
      }
      yyStates[yyTop] = yyState;
      yyVals[yyTop] = yyVal;
//t      if (debug != null) debug.push(yyState, yyVal);

      /*yyDiscarded:*/ while (true) {	// discarding a token does not change stack
        int yyN;
        if ((yyN = yyDefRed[yyState]) == 0) {	// else [default] reduce (yyN)
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
//t            if (debug != null)
//t              debug.lex(yyState, yyToken, yyname(yyToken), yyLex.value());
          }
          if ((yyN = yySindex[yyState]) != 0 && ((yyN += yyToken) >= 0)
              && (yyN < yyTable.Length) && (yyCheck[yyN] == yyToken)) {
//t            if (debug != null)
//t              debug.shift(yyState, yyTable[yyN], yyErrorFlag-1);
            yyState = yyTable[yyN];		// shift to yyN
            yyVal = yyLex.value();
            yyToken = -1;
            if (yyErrorFlag > 0) -- yyErrorFlag;
            goto continue_yyLoop;
          }
          if ((yyN = yyRindex[yyState]) != 0 && (yyN += yyToken) >= 0
              && yyN < yyTable.Length && yyCheck[yyN] == yyToken)
            yyN = yyTable[yyN];			// reduce (yyN)
          else
            switch (yyErrorFlag) {
  
            case 0:
              //yyExpectingState = yyState;
              Console.WriteLine(String.Format ("syntax error, got token `{0}' expecting: {1}",
                                yyname (yyToken),
                                String.Join(", ", yyExpecting(yyState))));
//t              if (debug != null) debug.error("syntax error");
              if (yyToken == 0 /*eof*/ || yyToken == eof_token) throw new yyParser.yyUnexpectedEof ();
              goto case 1;
            case 1: case 2:
              yyErrorFlag = 3;
              do {
                if ((yyN = yySindex[yyStates[yyTop]]) != 0
                    && (yyN += Token.yyErrorCode) >= 0 && yyN < yyTable.Length
                    && yyCheck[yyN] == Token.yyErrorCode) {
//t                  if (debug != null)
//t                    debug.shift(yyStates[yyTop], yyTable[yyN], 3);
                  yyState = yyTable[yyN];
                  yyVal = yyLex.value();
                  goto continue_yyLoop;
                }
//t                if (debug != null) debug.pop(yyStates[yyTop]);
              } while (-- yyTop >= 0);
//t              if (debug != null) debug.reject();
              throw new yyParser.yyException("irrecoverable syntax error");
  
            case 3:
              if (yyToken == 0) {
//t                if (debug != null) debug.reject();
                throw new yyParser.yyException("irrecoverable syntax error at end-of-file");
              }
//t              if (debug != null)
//t                debug.discard(yyState, yyToken, yyname(yyToken),
//t  							yyLex.value());
              yyToken = -1;
              goto continue_yyDiscarded;		// leave stack alone
            }
        }
        int yyV = yyTop + 1-yyLen[yyN];
//t        if (debug != null)
//t          debug.reduce(yyState, yyStates[yyV-1], yyN, YYRules.getRule (yyN), yyLen[yyN]);
        yyVal = yyV > yyTop ? null : yyVals[yyV]; // yyVal = yyDefault(yyV > yyTop ? null : yyVals[yyV]);
        switch (yyN) {
case 4:
#line 60 "Repil/IR/IR.jay"
  {
        module.SourceFilename = (string)yyVals[0+yyTop];
    }
  break;
case 5:
#line 64 "Repil/IR/IR.jay"
  {
        module.TargetDatalayout = (string)yyVals[0+yyTop];
    }
  break;
case 6:
#line 68 "Repil/IR/IR.jay"
  {
        module.TargetTriple = (string)yyVals[0+yyTop];
    }
  break;
case 7:
#line 72 "Repil/IR/IR.jay"
  {
        module.IdentifiedStructures[(Symbol)yyVals[-3+yyTop]] = (StructureType)yyVals[0+yyTop];
    }
  break;
case 8:
#line 76 "Repil/IR/IR.jay"
  {
        module.IdentifiedStructures[(Symbol)yyVals[-3+yyTop]] = OpaqueStructureType.Opaque;
    }
  break;
case 9:
  case_9();
  break;
case 10:
  case_10();
  break;
case 11:
  case_11();
  break;
case 13:
#line 96 "Repil/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-4+yyTop]] = new List<object> (0);
    }
  break;
case 14:
#line 100 "Repil/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-5+yyTop]] = yyVals[-1+yyTop];
    }
  break;
case 15:
  case_15();
  break;
case 16:
#line 109 "Repil/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-6+yyTop]] = yyVals[-1+yyTop];
    }
  break;
case 17:
  case_17();
  break;
case 18:
#line 121 "Repil/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 19:
#line 125 "Repil/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 20:
#line 129 "Repil/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: (bool)yyVals[-7+yyTop], isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 21:
#line 133 "Repil/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-6+yyTop], (LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], isPrivate: (bool)yyVals[-4+yyTop], isExternal: false, isConstant: (bool)yyVals[-2+yyTop]);
    }
  break;
case 22:
#line 137 "Repil/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-10+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: (bool)yyVals[-8+yyTop], isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 23:
#line 141 "Repil/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-3+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-6+yyTop], isConstant: (bool)yyVals[-4+yyTop]);
    }
  break;
case 24:
#line 145 "Repil/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-10+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: (bool)yyVals[-8+yyTop], isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 25:
#line 149 "Repil/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: (bool)yyVals[-7+yyTop], isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 26:
#line 153 "Repil/IR/IR.jay"
  { yyVal = false; }
  break;
case 27:
#line 154 "Repil/IR/IR.jay"
  { yyVal = true; }
  break;
case 28:
#line 158 "Repil/IR/IR.jay"
  { yyVal = true; }
  break;
case 29:
#line 159 "Repil/IR/IR.jay"
  { yyVal = false; }
  break;
case 30:
#line 163 "Repil/IR/IR.jay"
  { yyVal = true; }
  break;
case 31:
  case_31();
  break;
case 32:
  case_32();
  break;
case 33:
#line 180 "Repil/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 34:
#line 181 "Repil/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 35:
#line 182 "Repil/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 36:
#line 183 "Repil/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 37:
#line 184 "Repil/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 38:
#line 188 "Repil/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-5+yyTop], yyVals[-3+yyTop]);
    }
  break;
case 39:
#line 192 "Repil/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-4+yyTop], yyVals[-2+yyTop]);
    }
  break;
case 40:
#line 199 "Repil/IR/IR.jay"
  {
        yyVal = NewSyms (yyVals[-1+yyTop], (MetaSymbol)yyVals[0+yyTop]);
    }
  break;
case 41:
#line 203 "Repil/IR/IR.jay"
  {
        yyVal = SymsAdd (yyVals[-2+yyTop], yyVals[-1+yyTop], (MetaSymbol)yyVals[0+yyTop]);
    }
  break;
case 42:
#line 210 "Repil/IR/IR.jay"
  {
        yyVal = NewList (yyVals[0+yyTop]);
    }
  break;
case 43:
#line 214 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], yyVals[0+yyTop]);
    }
  break;
case 44:
#line 218 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 45:
#line 222 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 46:
#line 226 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 64:
#line 259 "Repil/IR/IR.jay"
  {
        yyVal = LiteralStructureType.Empty;
    }
  break;
case 65:
#line 263 "Repil/IR/IR.jay"
  {
        yyVal = new LiteralStructureType ((List<LType>)yyVals[-1+yyTop]);
    }
  break;
case 66:
#line 267 "Repil/IR/IR.jay"
  {
        yyVal = new PackedStructureType ((List<LType>)yyVals[-2+yyTop]);
    }
  break;
case 67:
#line 274 "Repil/IR/IR.jay"
  {
        yyVal = NewList ((LType)yyVals[0+yyTop]);
    }
  break;
case 68:
#line 278 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 70:
#line 283 "Repil/IR/IR.jay"
  { yyVal = VoidType.Void; }
  break;
case 73:
#line 289 "Repil/IR/IR.jay"
  { yyVal = FloatType.Half; }
  break;
case 74:
#line 290 "Repil/IR/IR.jay"
  { yyVal = FloatType.Float; }
  break;
case 75:
#line 291 "Repil/IR/IR.jay"
  { yyVal = FloatType.Double; }
  break;
case 76:
#line 292 "Repil/IR/IR.jay"
  { yyVal = FloatType.X86_FP80; }
  break;
case 77:
#line 296 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionType ((LType)yyVals[-2+yyTop], Enumerable.Empty<LType>());
    }
  break;
case 78:
#line 300 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionType ((LType)yyVals[-3+yyTop], (List<LType>)yyVals[-1+yyTop]);
    }
  break;
case 79:
#line 304 "Repil/IR/IR.jay"
  {
        yyVal = new PointerType ((LType)yyVals[-1+yyTop], 0);
    }
  break;
case 80:
#line 308 "Repil/IR/IR.jay"
  {
        yyVal = new NamedType ((Symbol)yyVals[0+yyTop]);
    }
  break;
case 81:
#line 312 "Repil/IR/IR.jay"
  {
        yyVal = new VectorType ((int)(BigInteger)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 82:
#line 316 "Repil/IR/IR.jay"
  {
        yyVal = new ArrayType ((long)(BigInteger)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 83:
#line 323 "Repil/IR/IR.jay"
  {
        yyVal = NewList ((LType)yyVals[0+yyTop]);
    }
  break;
case 84:
#line 327 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 86:
#line 335 "Repil/IR/IR.jay"
  {
        yyVal = VarArgsType.VarArgs;
    }
  break;
case 87:
#line 342 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDefinition ((LType)yyVals[-7+yyTop], (GlobalSymbol)yyVals[-6+yyTop], (IEnumerable<Parameter>)yyVals[-5+yyTop], (List<Block>)yyVals[-1+yyTop]);
    }
  break;
case 88:
#line 346 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDefinition ((LType)yyVals[-7+yyTop], (GlobalSymbol)yyVals[-6+yyTop], (IEnumerable<Parameter>)yyVals[-5+yyTop], (List<Block>)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[-3+yyTop]);
    }
  break;
case 89:
#line 350 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDefinition ((LType)yyVals[-8+yyTop], (GlobalSymbol)yyVals[-7+yyTop], (IEnumerable<Parameter>)yyVals[-6+yyTop], (List<Block>)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[-3+yyTop]);
    }
  break;
case 90:
#line 354 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDefinition ((LType)yyVals[-7+yyTop], (GlobalSymbol)yyVals[-6+yyTop], (IEnumerable<Parameter>)yyVals[-5+yyTop], (List<Block>)yyVals[-1+yyTop]);
    }
  break;
case 91:
#line 358 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDefinition ((LType)yyVals[-8+yyTop], (GlobalSymbol)yyVals[-7+yyTop], (IEnumerable<Parameter>)yyVals[-6+yyTop], (List<Block>)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[-3+yyTop]);
    }
  break;
case 92:
#line 362 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDefinition ((LType)yyVals[-6+yyTop], (GlobalSymbol)yyVals[-5+yyTop], (IEnumerable<Parameter>)yyVals[-4+yyTop], (List<Block>)yyVals[-1+yyTop]);
    }
  break;
case 93:
#line 366 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDefinition ((LType)yyVals[-7+yyTop], (GlobalSymbol)yyVals[-6+yyTop], (IEnumerable<Parameter>)yyVals[-5+yyTop], (List<Block>)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[-3+yyTop]);
    }
  break;
case 94:
#line 370 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDefinition ((LType)yyVals[-8+yyTop], (GlobalSymbol)yyVals[-7+yyTop], (IEnumerable<Parameter>)yyVals[-6+yyTop], (List<Block>)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[-3+yyTop]);
    }
  break;
case 95:
#line 374 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDefinition ((LType)yyVals[-7+yyTop], (GlobalSymbol)yyVals[-6+yyTop], (IEnumerable<Parameter>)yyVals[-5+yyTop], (List<Block>)yyVals[-1+yyTop]);
    }
  break;
case 96:
#line 378 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDefinition ((LType)yyVals[-8+yyTop], (GlobalSymbol)yyVals[-7+yyTop], (IEnumerable<Parameter>)yyVals[-6+yyTop], (List<Block>)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[-3+yyTop]);
    }
  break;
case 97:
#line 382 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDefinition ((LType)yyVals[-8+yyTop], (GlobalSymbol)yyVals[-7+yyTop], (IEnumerable<Parameter>)yyVals[-6+yyTop], (List<Block>)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[-3+yyTop]);
    }
  break;
case 98:
#line 389 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 99:
#line 393 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 100:
#line 397 "Repil/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-4+yyTop], (GlobalSymbol)yyVals[-3+yyTop], (IEnumerable<Parameter>)yyVals[-2+yyTop]);
    }
  break;
case 101:
#line 401 "Repil/IR/IR.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 102:
#line 402 "Repil/IR/IR.jay"
  { yyVal = Enumerable.Empty<Parameter> (); }
  break;
case 103:
#line 409 "Repil/IR/IR.jay"
  {
        yyVal = NewList ((Parameter)yyVals[0+yyTop]);
    }
  break;
case 104:
#line 413 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (Parameter)yyVals[0+yyTop]);
    }
  break;
case 105:
#line 420 "Repil/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, (LType)yyVals[0+yyTop]);
    }
  break;
case 106:
#line 424 "Repil/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, (LType)yyVals[-1+yyTop]);
    }
  break;
case 107:
#line 428 "Repil/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, IntegerType.I32);
    }
  break;
case 108:
#line 432 "Repil/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, VarArgsType.VarArgs);
    }
  break;
case 110:
#line 440 "Repil/IR/IR.jay"
  {
        yyVal = ((ParameterAttributes)yyVals[-1+yyTop]) | ((ParameterAttributes)yyVals[0+yyTop]);
    }
  break;
case 111:
#line 444 "Repil/IR/IR.jay"
  { yyVal = ParameterAttributes.NonNull; }
  break;
case 112:
#line 445 "Repil/IR/IR.jay"
  { yyVal = ParameterAttributes.NoCapture; }
  break;
case 113:
#line 446 "Repil/IR/IR.jay"
  { yyVal = ParameterAttributes.ReadOnly; }
  break;
case 114:
#line 447 "Repil/IR/IR.jay"
  { yyVal = ParameterAttributes.WriteOnly; }
  break;
case 115:
#line 448 "Repil/IR/IR.jay"
  { yyVal = ParameterAttributes.ReadNone; }
  break;
case 116:
#line 449 "Repil/IR/IR.jay"
  { yyVal = ParameterAttributes.SignExtend; }
  break;
case 117:
#line 450 "Repil/IR/IR.jay"
  { yyVal = ParameterAttributes.ZeroExtend; }
  break;
case 118:
#line 451 "Repil/IR/IR.jay"
  { yyVal = ParameterAttributes.Returned; }
  break;
case 124:
#line 469 "Repil/IR/IR.jay"
  { yyVal = IcmpCondition.Equal; }
  break;
case 125:
#line 470 "Repil/IR/IR.jay"
  { yyVal = IcmpCondition.NotEqual; }
  break;
case 126:
#line 471 "Repil/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedGreaterThan; }
  break;
case 127:
#line 472 "Repil/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedGreaterThanOrEqual; }
  break;
case 128:
#line 473 "Repil/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedLessThan; }
  break;
case 129:
#line 474 "Repil/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedLessThanOrEqual; }
  break;
case 130:
#line 475 "Repil/IR/IR.jay"
  { yyVal = IcmpCondition.SignedGreaterThan; }
  break;
case 131:
#line 476 "Repil/IR/IR.jay"
  { yyVal = IcmpCondition.SignedGreaterThanOrEqual; }
  break;
case 132:
#line 477 "Repil/IR/IR.jay"
  { yyVal = IcmpCondition.SignedLessThan; }
  break;
case 133:
#line 478 "Repil/IR/IR.jay"
  { yyVal = IcmpCondition.SignedLessThanOrEqual; }
  break;
case 134:
#line 482 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.True; }
  break;
case 135:
#line 483 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.False; }
  break;
case 136:
#line 484 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.Ordered; }
  break;
case 137:
#line 485 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedEqual; }
  break;
case 138:
#line 486 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedNotEqual; }
  break;
case 139:
#line 487 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedGreaterThan; }
  break;
case 140:
#line 488 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedGreaterThanOrEqual; }
  break;
case 141:
#line 489 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedLessThan; }
  break;
case 142:
#line 490 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedLessThanOrEqual; }
  break;
case 143:
#line 491 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.Unordered; }
  break;
case 144:
#line 492 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedEqual; }
  break;
case 145:
#line 493 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedNotEqual; }
  break;
case 146:
#line 494 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedGreaterThan; }
  break;
case 147:
#line 495 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedGreaterThanOrEqual; }
  break;
case 148:
#line 496 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedLessThan; }
  break;
case 149:
#line 497 "Repil/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedLessThanOrEqual; }
  break;
case 151:
#line 502 "Repil/IR/IR.jay"
  { yyVal = new LocalValue ((LocalSymbol)yyVals[0+yyTop]); }
  break;
case 152:
#line 503 "Repil/IR/IR.jay"
  { yyVal = new GlobalValue ((GlobalSymbol)yyVals[0+yyTop]); }
  break;
case 153:
#line 507 "Repil/IR/IR.jay"
  {
        yyVal = new IntToPointerValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 154:
#line 511 "Repil/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 155:
#line 515 "Repil/IR/IR.jay"
  {
        yyVal = new BitcastValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 156:
#line 519 "Repil/IR/IR.jay"
  {
        yyVal = new PtrtointValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 157:
#line 523 "Repil/IR/IR.jay"
  {
        yyVal = new VectorConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 158:
#line 527 "Repil/IR/IR.jay"
  {
        yyVal = new ArrayConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 159:
#line 531 "Repil/IR/IR.jay"
  {
        yyVal = new StructureConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 161:
#line 539 "Repil/IR/IR.jay"
  { yyVal = NullConstant.Null; }
  break;
case 162:
#line 540 "Repil/IR/IR.jay"
  { yyVal = new FloatConstant ((double)yyVals[0+yyTop]); }
  break;
case 163:
#line 541 "Repil/IR/IR.jay"
  { yyVal = new IntegerConstant ((BigInteger)yyVals[0+yyTop]); }
  break;
case 164:
#line 542 "Repil/IR/IR.jay"
  { yyVal = new HexIntegerConstant ((BigInteger)yyVals[0+yyTop]); }
  break;
case 165:
#line 543 "Repil/IR/IR.jay"
  { yyVal = BooleanConstant.True; }
  break;
case 166:
#line 544 "Repil/IR/IR.jay"
  { yyVal = BooleanConstant.False; }
  break;
case 167:
#line 545 "Repil/IR/IR.jay"
  { yyVal = UndefinedConstant.Undefined; }
  break;
case 168:
#line 546 "Repil/IR/IR.jay"
  { yyVal = ZeroConstant.Zero; }
  break;
case 169:
#line 547 "Repil/IR/IR.jay"
  { yyVal = new BytesConstant ((Symbol)yyVals[0+yyTop]); }
  break;
case 170:
#line 554 "Repil/IR/IR.jay"
  {
        yyVal = new LabelValue ((LocalSymbol)yyVals[0+yyTop]);
    }
  break;
case 171:
#line 561 "Repil/IR/IR.jay"
  {
        yyVal = new TypedValue ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 172:
#line 565 "Repil/IR/IR.jay"
  {
        yyVal = new TypedValue (VoidType.Void, VoidValue.Void);
    }
  break;
case 173:
#line 572 "Repil/IR/IR.jay"
  {
        yyVal = new TypedValue ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 174:
#line 579 "Repil/IR/IR.jay"
  {
        yyVal = NewList ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 175:
#line 583 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 176:
#line 590 "Repil/IR/IR.jay"
  {
        yyVal = new TypedConstant ((LType)yyVals[-1+yyTop], (Constant)yyVals[0+yyTop]);
    }
  break;
case 178:
#line 601 "Repil/IR/IR.jay"
  {
        yyVal = NewList ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 179:
#line 605 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 180:
#line 612 "Repil/IR/IR.jay"
  {
        yyVal = NewList ((Block)yyVals[0+yyTop]);
    }
  break;
case 181:
#line 616 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (Block)yyVals[0+yyTop]);
    }
  break;
case 182:
#line 623 "Repil/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, (List<Assignment>)yyVals[-1+yyTop], (TerminatorInstruction)yyVals[0+yyTop]);
    }
  break;
case 183:
#line 627 "Repil/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, (List<Assignment>)yyVals[-2+yyTop], (TerminatorInstruction)yyVals[-1+yyTop]);
    }
  break;
case 184:
#line 631 "Repil/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, Enumerable.Empty<Assignment>(), (TerminatorInstruction)yyVals[0+yyTop]);
    }
  break;
case 185:
#line 635 "Repil/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, Enumerable.Empty<Assignment>(), (TerminatorInstruction)yyVals[-1+yyTop]);
    }
  break;
case 186:
#line 642 "Repil/IR/IR.jay"
  {
        yyVal = NewList ((Assignment)yyVals[0+yyTop]);
    }
  break;
case 187:
#line 646 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (Assignment)yyVals[0+yyTop]);
    }
  break;
case 188:
#line 653 "Repil/IR/IR.jay"
  {
        yyVal = new Assignment ((Instruction)yyVals[0+yyTop]);
    }
  break;
case 189:
#line 657 "Repil/IR/IR.jay"
  {
        yyVal = new Assignment ((Instruction)yyVals[-1+yyTop]);
    }
  break;
case 190:
#line 661 "Repil/IR/IR.jay"
  {
        yyVal = new Assignment ((LocalSymbol)yyVals[-2+yyTop], (Instruction)yyVals[0+yyTop]);
    }
  break;
case 191:
#line 665 "Repil/IR/IR.jay"
  {
        yyVal = new Assignment ((LocalSymbol)yyVals[-3+yyTop], (Instruction)yyVals[-1+yyTop]);
    }
  break;
case 193:
#line 673 "Repil/IR/IR.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 194:
#line 674 "Repil/IR/IR.jay"
  { yyVal = Enumerable.Empty<Argument> (); }
  break;
case 195:
#line 681 "Repil/IR/IR.jay"
  {
        yyVal = NewList ((Argument)yyVals[0+yyTop]);
    }
  break;
case 196:
#line 685 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (Argument)yyVals[0+yyTop]);
    }
  break;
case 197:
#line 692 "Repil/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], (ParameterAttributes)0);
    }
  break;
case 198:
#line 696 "Repil/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], ParameterAttributes.NonNull);
    }
  break;
case 199:
#line 700 "Repil/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-1+yyTop], new LocalValue ((LocalSymbol)yyVals[0+yyTop]), (ParameterAttributes)0);
    }
  break;
case 200:
#line 704 "Repil/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], (ParameterAttributes)0);
    }
  break;
case 201:
#line 708 "Repil/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[0+yyTop]), (ParameterAttributes)0);
    }
  break;
case 202:
#line 712 "Repil/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[-2+yyTop]), (ParameterAttributes)0);
    }
  break;
case 203:
#line 716 "Repil/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[-3+yyTop]), (ParameterAttributes)0);
    }
  break;
case 205:
#line 721 "Repil/IR/IR.jay"
  { yyVal = new GlobalValue ((GlobalSymbol)yyVals[0+yyTop]); }
  break;
case 210:
#line 738 "Repil/IR/IR.jay"
  {
        yyVal = NewList ((PhiValue)yyVals[0+yyTop]);
    }
  break;
case 211:
#line 742 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (PhiValue)yyVals[0+yyTop]);
    }
  break;
case 212:
#line 748 "Repil/IR/IR.jay"
  {
        yyVal = new PhiValue ((Value)yyVals[-3+yyTop], (Value)yyVals[-1+yyTop]);
    }
  break;
case 213:
#line 755 "Repil/IR/IR.jay"
  {
        yyVal = NewList ((SwitchCase)yyVals[0+yyTop]);
    }
  break;
case 214:
#line 759 "Repil/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (SwitchCase)yyVals[0+yyTop]);
    }
  break;
case 215:
#line 766 "Repil/IR/IR.jay"
  {
        yyVal = new SwitchCase ((TypedConstant)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 221:
#line 784 "Repil/IR/IR.jay"
  { yyVal = AtomicConstraint.SequentiallyConsistent; }
  break;
case 222:
#line 791 "Repil/IR/IR.jay"
  {
        yyVal = new UnconditionalBrInstruction ((LabelValue)yyVals[0+yyTop]);
    }
  break;
case 223:
#line 795 "Repil/IR/IR.jay"
  {
        yyVal = new ConditionalBrInstruction ((Value)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 224:
#line 799 "Repil/IR/IR.jay"
  {
        yyVal = new RetInstruction ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 225:
#line 803 "Repil/IR/IR.jay"
  {
        yyVal = new SwitchInstruction ((TypedValue)yyVals[-5+yyTop], (LabelValue)yyVals[-3+yyTop], (List<SwitchCase>)yyVals[-1+yyTop]);
    }
  break;
case 226:
#line 810 "Repil/IR/IR.jay"
  {
        yyVal = new AddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 227:
#line 814 "Repil/IR/IR.jay"
  {
        yyVal = new AddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 228:
#line 818 "Repil/IR/IR.jay"
  {
        yyVal = new AllocaInstruction ((LType)yyVals[-3+yyTop], (int)(BigInteger)yyVals[0+yyTop]);
    }
  break;
case 229:
#line 822 "Repil/IR/IR.jay"
  {
        yyVal = new AndInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 230:
#line 826 "Repil/IR/IR.jay"
  {
        yyVal = new AshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], false);
    }
  break;
case 231:
#line 830 "Repil/IR/IR.jay"
  {
        yyVal = new AshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], true);
    }
  break;
case 232:
#line 834 "Repil/IR/IR.jay"
  {
        yyVal = new BitcastInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 233:
#line 838 "Repil/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 234:
#line 842 "Repil/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 235:
#line 846 "Repil/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 236:
#line 850 "Repil/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 237:
#line 854 "Repil/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 238:
#line 858 "Repil/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 239:
#line 862 "Repil/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 240:
#line 866 "Repil/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 241:
#line 870 "Repil/IR/IR.jay"
  {
        yyVal = new ExtractElementInstruction ((TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 242:
#line 874 "Repil/IR/IR.jay"
  {
        yyVal = new FaddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 243:
#line 878 "Repil/IR/IR.jay"
  {
        yyVal = new FcmpInstruction ((FcmpCondition)yyVals[-4+yyTop], (LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 244:
#line 882 "Repil/IR/IR.jay"
  {
        yyVal = new FdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 245:
#line 886 "Repil/IR/IR.jay"
  {
        yyVal = new FenceInstruction ((AtomicConstraint)yyVals[0+yyTop]);
    }
  break;
case 246:
#line 890 "Repil/IR/IR.jay"
  {
        yyVal = new FmulInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 247:
#line 894 "Repil/IR/IR.jay"
  {
        yyVal = new FpextInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 248:
#line 898 "Repil/IR/IR.jay"
  {
        yyVal = new FptouiInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 249:
#line 902 "Repil/IR/IR.jay"
  {
        yyVal = new FptosiInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 250:
#line 906 "Repil/IR/IR.jay"
  {
        yyVal = new FptruncInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 251:
#line 910 "Repil/IR/IR.jay"
  {
        yyVal = new FsubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 252:
#line 914 "Repil/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 253:
#line 918 "Repil/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 254:
#line 922 "Repil/IR/IR.jay"
  {
        yyVal = new IcmpInstruction ((IcmpCondition)yyVals[-4+yyTop], (LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 255:
#line 926 "Repil/IR/IR.jay"
  {
        yyVal = new InsertElementInstruction ((TypedValue)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 256:
#line 930 "Repil/IR/IR.jay"
  {
        yyVal = new InttoptrInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 257:
#line 934 "Repil/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: false);
    }
  break;
case 258:
#line 938 "Repil/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: true);
    }
  break;
case 259:
#line 942 "Repil/IR/IR.jay"
  {
        yyVal = new LshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], false);
    }
  break;
case 260:
#line 946 "Repil/IR/IR.jay"
  {
        yyVal = new LshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], true);
    }
  break;
case 261:
#line 950 "Repil/IR/IR.jay"
  {
        yyVal = new OrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 262:
#line 954 "Repil/IR/IR.jay"
  {
        yyVal = new MultiplyInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 263:
#line 958 "Repil/IR/IR.jay"
  {
        yyVal = new MultiplyInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 264:
#line 962 "Repil/IR/IR.jay"
  {
        yyVal = new PhiInstruction ((LType)yyVals[-1+yyTop], (List<PhiValue>)yyVals[0+yyTop]);
    }
  break;
case 265:
#line 966 "Repil/IR/IR.jay"
  {
        yyVal = new PtrtointInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 266:
#line 970 "Repil/IR/IR.jay"
  {
        yyVal = new SdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 267:
#line 974 "Repil/IR/IR.jay"
  {
        yyVal = new SelectInstruction ((LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 268:
#line 978 "Repil/IR/IR.jay"
  {
        yyVal = new SextInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 269:
#line 982 "Repil/IR/IR.jay"
  {
        yyVal = new ShlInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 270:
#line 986 "Repil/IR/IR.jay"
  {
        yyVal = new ShlInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 271:
#line 990 "Repil/IR/IR.jay"
  {
        yyVal = new ShuffleVectorInstruction ((TypedValue)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 272:
#line 994 "Repil/IR/IR.jay"
  {
        yyVal = new SitofpInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 273:
#line 998 "Repil/IR/IR.jay"
  {
        yyVal = new SremInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 274:
#line 1002 "Repil/IR/IR.jay"
  {
        yyVal = new StoreInstruction (value: (TypedValue)yyVals[-5+yyTop], pointer: (TypedValue)yyVals[-3+yyTop], isVolatile: false);
    }
  break;
case 275:
#line 1006 "Repil/IR/IR.jay"
  {
        yyVal = new StoreInstruction (value: (TypedValue)yyVals[-5+yyTop], pointer: (TypedValue)yyVals[-3+yyTop], isVolatile: true);
    }
  break;
case 276:
#line 1010 "Repil/IR/IR.jay"
  {
        yyVal = new SubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 277:
#line 1014 "Repil/IR/IR.jay"
  {
        yyVal = new SubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 278:
#line 1018 "Repil/IR/IR.jay"
  {
        yyVal = new TruncInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 279:
#line 1022 "Repil/IR/IR.jay"
  {
        yyVal = new UdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 280:
#line 1026 "Repil/IR/IR.jay"
  {
        yyVal = new UitofpInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 281:
#line 1030 "Repil/IR/IR.jay"
  {
        yyVal = new UremInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 282:
#line 1034 "Repil/IR/IR.jay"
  {
        yyVal = new XorInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 283:
#line 1038 "Repil/IR/IR.jay"
  {
        yyVal = new ZextInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
#line default
        }
        yyTop -= yyLen[yyN];
        yyState = yyStates[yyTop];
        int yyM = yyLhs[yyN];
        if (yyState == 0 && yyM == 0) {
//t          if (debug != null) debug.shift(0, yyFinal);
          yyState = yyFinal;
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
//t            if (debug != null)
//t               debug.lex(yyState, yyToken,yyname(yyToken), yyLex.value());
          }
          if (yyToken == 0) {
//t            if (debug != null) debug.accept(yyVal);
            return yyVal;
          }
          goto continue_yyLoop;
        }
        if (((yyN = yyGindex[yyM]) != 0) && ((yyN += yyState) >= 0)
            && (yyN < yyTable.Length) && (yyCheck[yyN] == yyState))
          yyState = yyTable[yyN];
        else
          yyState = yyDgoto[yyM];
//t        if (debug != null) debug.shift(yyStates[yyTop], yyState);
	 goto continue_yyLoop;
      continue_yyDiscarded: ;	// implements the named-loop continue: 'continue yyDiscarded'
      }
    continue_yyLoop: ;		// implements the named-loop continue: 'continue yyLoop'
    }
  }

/*
 All more than 3 lines long rules are wrapped into a method
*/
void case_9()
#line 78 "Repil/IR/IR.jay"
{
        var f = (FunctionDefinition)yyVals[0+yyTop];
        module.FunctionDefinitions[f.Symbol] = f;
    }

void case_10()
#line 83 "Repil/IR/IR.jay"
{
        var f = (FunctionDeclaration)yyVals[0+yyTop];
        module.FunctionDeclarations[f.Symbol] = f;
    }

void case_11()
#line 88 "Repil/IR/IR.jay"
{
        var g = (GlobalVariable)yyVals[0+yyTop];
        module.GlobalVariables[g.Symbol] = g;
    }

void case_15()
#line 102 "Repil/IR/IR.jay"
{
        var m = SymsAdd (yyVals[-1+yyTop], Symbol.Intern("_f"), yyVals[-3+yyTop]);
        module.Metadata[(Symbol)yyVals[-5+yyTop]] = m;
    }

void case_17()
#line 111 "Repil/IR/IR.jay"
{
        var m = SymsAdd (yyVals[-1+yyTop], Symbol.Intern("_f"), yyVals[-3+yyTop]);
        module.Metadata[(Symbol)yyVals[-6+yyTop]] = m;
    }

void case_31()
#line 168 "Repil/IR/IR.jay"
{
        var t = (Tuple<object, object>)yyVals[0+yyTop];
        yyVal = NewSyms (t.Item1, t.Item2);
    }

void case_32()
#line 173 "Repil/IR/IR.jay"
{
        var t = (Tuple<object, object>)yyVals[0+yyTop];
        yyVal = SymsAdd (yyVals[-2+yyTop], t.Item1, t.Item2);
    }

#line default
   static readonly short [] yyLhs  = {              -1,
    0,    1,    1,    2,    2,    2,    2,    2,    2,    2,
    2,    2,    2,    2,    2,    2,    2,    6,    6,    6,
    6,    6,    6,    6,    6,   10,   10,   16,   16,   15,
    9,    9,   17,   17,   17,   17,   17,   17,   17,   13,
   13,    8,    8,    8,    8,    8,   20,   20,   20,    7,
    7,   22,   22,   22,   22,   22,   22,   22,   22,   22,
   22,   22,   22,    3,    3,    3,   23,   23,   24,   24,
   11,   11,   11,   11,   11,   11,   11,   11,   11,   11,
   11,   11,   25,   25,   26,   26,    4,    4,    4,    4,
    4,    4,    4,    4,    4,    4,    4,    5,    5,    5,
   27,   27,   32,   32,   33,   33,   33,   33,   34,   34,
   31,   31,   31,   31,   31,   31,   31,   31,   14,   14,
   28,   28,   35,   36,   36,   36,   36,   36,   36,   36,
   36,   36,   36,   37,   37,   37,   37,   37,   37,   37,
   37,   37,   37,   37,   37,   37,   37,   37,   37,   12,
   12,   12,   12,   12,   12,   12,   12,   12,   12,   40,
   18,   18,   18,   18,   18,   18,   18,   18,   18,   41,
   21,   21,   42,   39,   39,   43,   44,   38,   38,   29,
   29,   45,   45,   45,   45,   46,   46,   48,   48,   48,
   48,   50,   51,   51,   52,   52,   53,   53,   53,   53,
   53,   53,   53,   54,   54,   19,   19,   55,   55,   56,
   56,   57,   58,   58,   59,   60,   60,   61,   61,   30,
   62,   47,   47,   47,   47,   49,   49,   49,   49,   49,
   49,   49,   49,   49,   49,   49,   49,   49,   49,   49,
   49,   49,   49,   49,   49,   49,   49,   49,   49,   49,
   49,   49,   49,   49,   49,   49,   49,   49,   49,   49,
   49,   49,   49,   49,   49,   49,   49,   49,   49,   49,
   49,   49,   49,   49,   49,   49,   49,   49,   49,   49,
   49,   49,   49,
  };
   static readonly short [] yyLen = {           2,
    1,    1,    2,    3,    4,    4,    4,    4,    1,    1,
    1,    6,    5,    6,    6,    7,    7,    9,   10,   10,
    7,   11,    9,   11,   10,    1,    1,    1,    1,    1,
    1,    3,    3,    3,    3,    3,    3,    6,    5,    2,
    3,    1,    2,    3,    3,    3,    1,    1,    1,    1,
    2,    1,    1,    1,    1,    1,    1,    1,    3,    1,
    1,    1,    4,    2,    3,    5,    1,    3,    1,    1,
    1,    1,    1,    1,    1,    1,    3,    4,    2,    1,
    5,    5,    1,    3,    1,    1,    9,    9,   10,   10,
   11,    9,   10,   11,   11,   13,   12,    5,    6,    6,
    3,    2,    1,    3,    1,    2,    1,    1,    1,    2,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    2,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    6,    9,    6,    6,    3,    3,    3,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    2,
    2,    1,    2,    1,    3,    2,    1,    1,    3,    1,
    2,    2,    3,    1,    2,    1,    2,    1,    2,    3,
    4,    1,    3,    2,    1,    3,    2,    3,    3,    3,
    2,    4,    5,    1,    1,    1,    3,    1,    1,    1,
    3,    5,    1,    2,    3,    1,    2,    1,    1,    1,
    1,    2,    7,    2,    7,    5,    6,    5,    5,    5,
    6,    4,    4,    5,    6,    5,    6,    5,    6,    7,
    4,    5,    6,    5,    2,    5,    4,    4,    4,    4,
    5,    6,    7,    6,    6,    4,    7,    8,    5,    6,
    5,    5,    6,    3,    4,    5,    7,    4,    5,    6,
    6,    4,    5,    7,    8,    5,    6,    4,    5,    4,
    5,    5,    4,
  };
   static readonly short [] yyDefRed = {            0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    2,    9,   10,   11,    0,    0,    0,    0,    0,    0,
   70,   80,   73,   74,   75,   76,   72,    0,   29,   28,
    0,    0,    0,   71,    0,    0,    0,    0,    0,    0,
    3,    4,    0,    0,  119,  120,   26,   27,   30,    0,
    0,    0,    0,    0,    0,    0,    0,    0,   64,    0,
    0,    0,    0,    0,    0,   79,  220,    0,    0,    0,
    0,    0,    0,    0,    5,    6,    0,    0,    0,    0,
    0,    8,    0,    7,    0,    0,    0,    0,    0,   65,
    0,    0,    0,    0,    0,  116,  117,  118,  111,  112,
  114,  113,  115,    0,    0,    0,    0,   86,   77,    0,
    0,   83,    0,    0,    0,  163,  164,  162,  165,  166,
  167,  161,  152,  151,  169,  168,    0,    0,    0,    0,
    0,    0,    0,    0,  150,    0,    0,    0,    0,    0,
    0,    0,   31,    0,    0,    0,   49,   48,   13,    0,
    0,   42,   47,    0,    0,    0,    0,    0,    0,    0,
    0,  107,  108,  102,    0,    0,  103,  123,    0,    0,
  121,   78,    0,    0,    0,    0,    0,    0,   62,   54,
   52,   53,   55,   56,   57,   58,    0,   50,    0,    0,
    0,    0,  174,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,   15,    0,    0,    0,   43,   14,    0,
  171,    0,   81,   66,   82,    0,    0,    0,    0,  109,
    0,  101,    0,    0,    0,    0,  122,   84,    0,    0,
    0,    0,   12,   51,    0,    0,    0,    0,  159,    0,
  157,  158,    0,    0,    0,    0,    0,    0,   35,    0,
   33,   36,   37,   32,   17,   16,   46,   45,   44,    0,
    0,    0,    0,    0,    0,  110,  104,    0,    0,   40,
    0,    0,   59,  209,  208,    0,  206,    0,    0,    0,
    0,  175,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  180,    0,    0,  186,    0,    0,    0,    0,    0,    0,
   41,    0,   63,    0,    0,    0,    0,    0,    0,    0,
    0,    0,   23,    0,   39,    0,    0,    0,    0,    0,
  224,    0,    0,  222,    0,  218,  219,    0,    0,  216,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  221,
  245,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  124,  125,  126,  127,  128,
  129,  130,  131,  132,  133,    0,  134,  135,  146,  147,
  148,  149,  137,  139,  140,  141,  142,  138,  136,  144,
  145,  143,    0,    0,    0,    0,    0,    0,   92,  181,
    0,  187,    0,    0,    0,    0,    0,    0,   87,    0,
   88,  207,    0,  156,  153,  155,    0,    0,    0,    0,
   38,   90,    0,    0,    0,  170,    0,    0,    0,    0,
  217,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  210,    0,
  192,    0,    0,    0,    0,    0,    0,   93,    0,    0,
    0,   89,    0,    0,    0,   91,   94,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  241,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,   95,    0,
    0,  177,    0,  178,    0,    0,  226,    0,  242,  276,
    0,  251,  262,    0,  246,  279,  266,  244,  281,  273,
  269,    0,    0,  259,    0,  230,  229,  261,  282,    0,
    0,  228,    0,  160,  173,    0,    0,    0,    0,    0,
    0,    0,    0,  211,    0,    0,  194,    0,    0,  195,
    0,  234,    0,    0,    0,    0,   97,    0,  154,    0,
    0,    0,    0,    0,  213,  227,  277,  263,  270,  260,
  231,  255,  271,    0,    0,    0,    0,    0,    0,  254,
  243,    0,    0,    0,    0,  197,    0,  193,    0,  235,
    0,  239,    0,   96,  179,  223,  176,    0,  225,  214,
    0,  257,    0,  274,    0,  212,  267,    0,  205,  199,
  204,  200,  198,  196,  240,  215,  258,  275,  202,    0,
  203,
  };
  protected static readonly short [] yyDgoto  = {             9,
   10,   11,   34,   12,   13,   14,  187,  150,  142,   50,
  151,  541,  226,   51,   52,   36,  143,  135,  276,  152,
  622,  188,   61,   62,  111,  112,  107,  170,  340,   69,
  220,  166,  167,  221,  171,  436,  453,  623,  194,  655,
  374,  590,  683,  624,  341,  342,  343,  344,  345,  542,
  613,  669,  670,  732,  277,  538,  539,  684,  685,  379,
  380,  411,
  };
  protected static readonly short [] yySindex = {          412,
   47, -178,   69,   74,   81, 3522, 3579, -241,    0,  412,
    0,    0,    0,    0, -110,   83,   97,  450, -114,  -20,
    0,    0,    0,    0,    0,    0,    0, 3727,    0,    0,
 3621, -109,  -77,    0,  157, 3497,  -25, 3727,  -17,  127,
    0,    0,  -46,    5,    0,    0,    0,    0,    0, 3727,
 -176, -116,   29,  -48,  200,  -26,  212,  -16,    0,  157,
  -28,  250,   41, 3727,   99,    0,    0,  -11, 3353,  269,
 2058,   -3,  269,  245,    0,    0, 1910, 3727, -176, 3727,
 -176,    0,  248,    0, -200,  327,  249, 3614,  269,    0,
 3727, 3727,    4, 3727,  269,    0,    0,    0,    0,    0,
    0,    0,    0,    9, 3727, 2043, -168,    0,    0,  157,
  166,    0,  269, -168,  566,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,   16,  335,  336,  342,
 3732, 3732, 3732,  339,    0, 1910, 3727, 1910, 3727,  326,
  338,  167,    0, -200, 3608,    0,    0,    0,    0,    2,
 1910,    0,    0, -116,  157,   42,  323,    3, -168,  269,
   10,    0,    0,    0,  328,  172,    0,    0,   77, -222,
    0,    0, 3564,   77,   77,   77,  332,  354,    0,    0,
    0,    0,    0,    0,    0,    0,  442,    0,  355, 3732,
 3732, 3732,    0,    7,   72,   13,   35,  363, 1910,  371,
 1864, 4837,  124,    0, -200,  177,   12,    0,    0, 3649,
    0,   77,    0,    0,    0,   77, -113, -116,  269,    0,
  424,    0, 3549, -112,  125, -106,    0,    0,   77,   77,
  159, 2170,    0,    0, 3727,   51,   52,   53,    0, 3732,
    0,    0,  174,   67,  391,   75,   76,  392,    0,  403,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -104,
 -222, 4307, -102,   77, -116,    0,    0, 4307,  -96,    0,
  168, 4307,    0,    0,    0,  184,    0,   39, 3727, 3727,
 3727,    0,  170,  191,   87,  206,  207,  105,    1, 4307,
  -92,  -85,  398, 3732, -161, 3732, 1939, 3727, 1939, 3727,
 1939, 3727, 3727, 3727, 3727, 3727, 3727, 1939,  -32,  629,
 3727, 3727, 3727, 3732, 3732, 3732, 3727, 2010, 3451,  152,
  603, 3732, 3732, 3732, 3732, 3732, 3732, 3732, 3732, 3732,
 3732, 3732, 3732, 2216, 3663, 3727, 3727, 3497,   82, 2035,
    0, 4307,  170,    0,  170, 4307, -103,   77, 2125, 4307,
    0, 2205,    0, 2170, 3732,  178,  229,  242,  196,  170,
  220,  170,    0,  227,    0,  186, 2292, 4307, 4307, 4703,
    0,  213, 1986,    0,  443,    0,    0, 1910, 1939,    0,
 1910, 1910, 1939, 1910, 1910, 1939, 1910, 1910, 1910, 1910,
 1910, 1910, 1910, 1939, 3727, 1910, 3727, 1910, 1910, 1910,
 1910,  444,  445,  446,  150, 3727,  274, 3732,  449,    0,
    0, 3727,  283,  126,  128,  129,  130,  131,  132,  134,
  135,  137,  139,  140,  141,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0, 3727,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0, 3727,   18, 1910,  155, 3353, 3497,    0,    0,
  170,    0,  196,  196, 2381, 4307,  -83, -222,    0, 2461,
    0,    0,  471,    0,    0,    0,  196,  170,  196,  170,
    0,    0, 2548, 2637,  170,    0,  490,  228,  491, 1910,
    0,  493,  495, 1910,  497,  504, 1910,  505,  506,  507,
  508,  509,  511,  515, 1910, 1910,  516, 1910,  519,  521,
  522,  524, 3732, 3732, 3732,  208,  301, 3727,  527, 3727,
  308, 3732, 3727, 3727, 3727, 3727, 3727, 3727, 3727, 3727,
 3727, 3727, 3727, 3727, 1910, 1910, 1986,  529,    0,  530,
    0,  539,  155, 3727,  155, 3353,  196,    0, 2717, 4307,
  -76,    0, 3732,  196,  196,    0,    0,  196,  228,  513,
 1986,  537, 1986, 1986,  544, 1986, 1986,  552, 1986, 1986,
 1986, 1986, 1986, 1986, 1986,  553,  561, 1986,  563, 1986,
 1986, 1986, 1986,    0,  564,  565,  353, 3727, 1910,  567,
 3727,  569, 3732,  570,  157,  157,  157,  157,  157,  157,
  157,  157,  157,  157,  157,  157,  571,  575,  579,  548,
 3732, 3412,   77,  539,  155,  539,  155, 3727,    0, 2804,
 4307,    0,  192,    0,  597, 3727,    0, 1986,    0,    0,
 1986,    0,    0, 1986,    0,    0,    0,    0,    0,    0,
    0, 1986, 1986,    0, 1986,    0,    0,    0,    0, 3732,
 3732,    0,  598,    0,    0,  282,  601,  287,  608, 3732,
 1986, 1986, 1986,    0,  610, 3654,    0, 1786,  244,    0,
   77,    0,  539,   77,  539,  155,    0, 2893,    0, 3732,
  228, 1752,  611, 3678,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  294,  401,  299,  405, 3732,  620,    0,
    0,  572, 3732,  628, 1672,    0, 1929,    0, 3692,    0,
   77,    0,  539,    0,    0,    0,    0,  228,    0,    0,
  415,    0,  416,    0,  620,    0,    0,  531,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  273,
    0,
  };
  protected static readonly short [] yyRindex = {            0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  669,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0, 1706,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,   22,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  636,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  116,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  636,    0,  636,    0,    0,
    0,    0,    0,    0,    0,  586,    0,    0,    0,    0,
  636,    0,    0,    0,   58,  636,    0,  636,    0,    0,
    0,    0,    0,    0,  146,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  136, 3711, 3726,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  636,    0,
  636,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  293,    0,    0,    0,    0,    0,    0,    0,  185,  193,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  203,    0,    0,    0,    0,  298,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  636,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0, 2973,    0, 4387,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  636,  636,  636,  289,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  636,    0,    0,
  636,  636,    0,  636,  636,    0,  636,  636,  636,  636,
  636,  636,  636,    0,    0,  636,    0,  636,  636,  636,
  636,    0,    0,    0,  636,    0,  636,    0,    0,    0,
    0,    0,  636,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  636,  636,    0,    0,    0,    0,    0,
 3060,    0, 3149, 4467,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  349,  378,  402,    0,
    0,    0,    0,    0, 4547,    0,    0,    0,    0,  636,
    0,    0,    0,  636,    0,    0,  636,    0,    0,    0,
    0,    0,    0,    0,  636,  636,    0,  636,    0,    0,
    0,    0,    0,    0,    0,    0,  636,    0,    0,    0,
  636,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  636,  636,    0, 3747,    0,    0,
    0,    0,    0,    0,    0,    0, 3229,    0,    0,    0,
    0,    0,    0,  533,  543,    0,    0, 4627,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  636,    0,
    0,    0,    0,    0,  666,  746,  826,  906,  986, 1066,
 1146, 1226, 1306, 1386, 1466, 1546,    0,    0,    0,    0,
    0,    0, 3827,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  636,    0,    0,
 3907,    0,    0, 3987,    0,    0,    0,    0,    0,    0,
    0,  636,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0, 4067,    0,
    0,    0,    0,  300,  636,    0,    0,    0,    0,    0,
 4147,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0, 4227,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,
  };
  protected static readonly short [] yyGindex = {            0,
    0,  676,  639,    0,    0,    0,    0,  550,  554,  100,
   -6,  202, 1795,  215,    0,  679,  499, -199, -283,    0,
   32,  518,  645,   -2,    0,  540,  337, -105, -229, -320,
  -67,    0,  492,   46, -135,    0,    0, -583,  102,    0,
 -480, -442,    0,   36, -239,    0,  375,  377,  351, -472,
 -561,    0,   14,    0,  379,    0,  133,    0,   50,  -99,
 -210,    0,
  };
  protected static readonly short [] yyTable = {            35,
   35,  105,  252,   37,   39,  366,   87,  560,  176,  262,
  268,   83,   57,   64,   71,   91,  272,  457,  290,  466,
  346,   35,   71,   71,   60,   58,  350,   32,   71,   35,
  368,   35,  275,   68,  227,   72,   71,  369,  349,  550,
  227,  365,  352,   77,   66,  210,  621,   91,   71,   71,
  240,  225,  672,  217,  674,  210,  240,   60,   33,   66,
  367,   69,   35,  224,  110,   67,  104,   40,  229,  230,
  614,  136,  616,  138,   31,  140,  699,  592,  625,  141,
   66,  227,  355,   66,  155,  156,  168,  158,  227,  275,
   31,   16,   17,  227,  227,  215,   90,   69,   35,  165,
  460,   68,  161,  213,  372,  242,  260,   15,  537,  460,
  261,  710,  460,  712,  725,  240,  465,   47,   48,  153,
  470,   45,   46,  373,  227,  227,  209,  460,  157,   18,
  199,  239,  201,  241,   19,   98,  256,  546,  483,  484,
  168,   20,  673,   43,  675,  653,   67,   63,  657,   42,
   78,  735,   80,  266,  275,   69,   85,   44,  347,   85,
  225,  225,  193,  193,  193,   54,  110,  271,  491,  225,
  225,  271,  491,   45,   46,  491,  153,  271,  137,   65,
  139,  271,   68,  491,   99,   69,  105,   74,  271,  105,
  271,   66,  100,  516,   71,  168,  168,  271,   66,  383,
  716,  386,   21,  713,  168,  168,  172,  204,  394,  173,
  205,  227,  222,   75,  132,  223,  165,  255,  474,   66,
  205,  236,  237,  238,  353,  460,  481,  354,  278,  354,
  460,   21,  679,  195,  196,  680,  549,  736,   82,   85,
   22,  259,  468,  460,  460,  133,   70,   86,   23,   24,
   25,   26,   27,   55,   73,   89,   56,  116,  117,  118,
   95,  119,  120,  121,   76,  122,   79,   81,  113,  475,
   66,  282,  356,  357,  358,  208,  274,  131,  134,  125,
  160,  219,  476,   66,  708,  208,  126,  709,   18,   71,
  378,  381,  382,  384,  385,  387,  388,  389,  390,  391,
  392,  393,  396,  398,  399,  400,  401,   92,  106,  460,
  405,  407,  395,  741,  413,   66,  354,  518,   45,   46,
  620,  169,   47,   48,   66,  371,  522,  375,  175,  454,
  455,   35,  227,  106,   88,  456,  106,  198,   34,  200,
  201,   34,   66,  201,  588,  402,  403,  404,   19,   66,
  409,  593,  211,  414,  415,  416,  417,  418,  419,  420,
  421,  422,  423,  424,  425,   94,  144,  115,  212,   66,
   64,  145,  490,  216,  190,  191,  494,   20,  189,  497,
  460,  192,  197,  202,  214,  168,  473,  505,  506,  544,
  508,  678,  231,  232,  235,  203,  243,  253,  270,  517,
  245,   25,  248,   98,   98,  521,  244,   98,   98,  114,
   98,  116,  117,  118,  246,  119,  120,  121,  273,  122,
  279,  280,  281,   98,   98,  154,  123,  124,  284,  535,
  283,  159,  264,  125,  285,  288,  286,  287,  460,  519,
  126,  351,  289,  225,  740,   98,  536,  360,  361,  174,
   35,   35,   99,   99,  543,  545,   99,   99,  370,   99,
  100,  100,  362,  363,  100,  100,  364,  100,  410,  271,
   21,   21,   99,   99,   21,   21,  478,   21,  618,  348,
  100,  100,  717,  480,  458,  486,  488,  513,  514,  515,
   21,   21,  520,  372,   99,  523,  218,  524,  525,  526,
  527,  528,  100,  529,  530,  731,  531,  671,  532,  533,
  534,  589,   21,  589,  553,  127,  595,  596,  597,  598,
  599,  600,  601,  602,  603,  604,  605,  606,  275,  128,
  129,  130,   22,  559,  561,  227,  563,   35,  564,   35,
  566,  615,   24,  617,  584,  585,  586,  567,  569,  570,
  571,  572,  573,  594,  574,  265,   18,   18,  575,  578,
   18,   18,  580,   18,  581,  582,  233,  583,  711,  587,
  591,  739,  610,  611,  487,  227,   18,   18,  612,  489,
  628,  589,  492,  493,  589,  495,  496,  631,  498,  499,
  500,  501,  502,  503,  504,  634,  642,  507,   18,  509,
  510,  511,  512,  626,  643,  668,  645,  650,  651,  652,
  656,   35,  658,  660,  661,  676,   19,   19,  662,  682,
   19,   19,  663,   19,  659,   70,  172,   96,   97,  172,
   98,   99,  100,  101,  102,  103,   19,   19,  537,  266,
  681,  694,  665,  695,  696,   20,   20,  172,  697,   20,
   20,  698,   20,  703,  718,  721,  540,  722,   19,  705,
  723,  724,   32,  680,  726,   20,   20,  728,    1,   25,
   25,  737,  738,   25,   25,   69,   25,  682,  172,    1,
    2,  692,  693,    3,    4,   41,    5,   20,   32,   25,
   25,  562,   84,   33,  207,  565,   53,  206,  568,    6,
    7,  177,  668,  254,  234,   69,  576,  577,   93,  579,
  172,   25,  228,  707,  267,  715,  461,  178,  462,   33,
  485,    8,  734,   96,   97,   31,   98,   99,  100,  101,
  102,  103,  472,  720,  727,    0,  607,  608,  609,   45,
   46,    0,  664,   47,   48,   49,   29,   30,  179,  180,
    0,   31,  181,  182,  183,  184,  185,  186,    0,    0,
    0,    0,  627,    0,  629,  630,    0,  632,  633,    0,
  635,  636,  637,  638,  639,  640,  641,    0,    0,  644,
    0,  646,  647,  648,  649,   69,    0,  116,  117,  118,
  654,  119,  120,  121,    0,  122,    0,    0,    0,    0,
   22,   22,    0,    0,   22,   22,  274,   22,    0,  125,
   24,   24,    0,    0,   24,   24,  126,   24,    0,    0,
   22,   22,    0,    0,    0,  177,    0,    0,    0,  686,
   24,   24,  687,    0,    0,  688,    0,    0,    0,    0,
    0,  178,   22,  689,  690,    0,  691,    0,    0,    0,
    0,    0,   24,    0,    0,    0,    0,    0,  172,  172,
    0,    0,  700,  701,  702,   69,   21,    0,    0,  706,
    0,    0,  179,  180,    0,   22,  181,  182,  183,  184,
  185,  186,    0,   23,   24,   25,   26,   27,    0,    0,
    0,    0,   21,    0,    0,    0,    0,    0,    0,    0,
    0,   22,    0,  172,  172,  172,    0,    0,  733,   23,
   24,   25,   26,   27,  172,    0,    0,  172,  172,  172,
  172,  172,  172,  172,  172,  172,  172,    0,  172,  172,
    0,  172,  172,  172,  172,  172,  172,  172,  278,  278,
  172,  172,  172,  172,    0,   69,  172,    0,    0,    0,
  172,  172,  172,  172,  172,  172,  172,  172,  172,  172,
  172,  172,  172,    0,  172,  412,    0,    0,    0,    0,
    0,    0,    0,  397,    0,  172,    0,    0,    0,    0,
    0,    0,    0,  278,  278,  278,  172,  172,  172,  172,
    0,    0,    0,    0,  278,    0,    0,  278,  278,  278,
  278,  278,  278,  278,  278,  278,  278,    0,  278,  278,
    0,  278,  278,  278,  278,  278,  278,  278,  283,  283,
  278,  278,  278,  278,    0,   69,  278,    0,    0,    0,
  278,  278,  278,  278,  278,    0,  278,  278,  278,  278,
  278,  278,  278,    0,  278,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  278,    0,    0,    0,    0,
    0,    0,    0,  283,  283,  283,  278,  278,  278,  278,
    0,    0,    0,    0,  283,    0,    0,  283,  283,  283,
  283,  283,  283,  283,  283,  283,  283,    0,  283,  283,
    0,  283,  283,  283,  283,  283,  283,  283,  268,  268,
  283,  283,  283,  283,    0,   69,  283,    0,    0,    0,
  283,  283,  283,  283,  283,    0,  283,  283,  283,  283,
  283,  283,  283,    0,  283,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  283,    0,    0,    0,    0,
    0,    0,    0,  268,  268,  268,  283,  283,  283,  283,
    0,    0,    0,    0,  268,    0,    0,  268,  268,  268,
  268,  268,  268,  268,  268,  268,  268,    0,  268,  268,
    0,  268,  268,  268,  268,  268,  268,  268,  250,  250,
  268,  268,  268,  268,    0,   69,  268,    0,    0,    0,
  268,  268,  268,  268,  268,    0,  268,  268,  268,  268,
  268,  268,  268,    0,  268,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  268,    0,    0,    0,    0,
    0,    0,    0,  250,  250,  250,  268,  268,  268,  268,
    0,    0,    0,    0,  250,    0,    0,  250,  250,  250,
  250,  250,  250,  250,  250,  250,  250,    0,  250,  250,
    0,  250,  250,  250,  250,  250,  250,  250,  247,  247,
  250,  250,  250,  250,    0,   69,  250,    0,    0,    0,
  250,  250,  250,  250,  250,    0,  250,  250,  250,  250,
  250,  250,  250,    0,  250,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  250,    0,    0,    0,    0,
    0,    0,    0,  247,  247,  247,  250,  250,  250,  250,
    0,    0,    0,    0,  247,    0,    0,  247,  247,  247,
  247,  247,  247,  247,  247,  247,  247,    0,  247,  247,
    0,  247,  247,  247,  247,  247,  247,  247,  248,  248,
  247,  247,  247,  247,    0,   69,  247,    0,    0,    0,
  247,  247,  247,  247,  247,    0,  247,  247,  247,  247,
  247,  247,  247,    0,  247,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  247,    0,    0,    0,    0,
    0,    0,    0,  248,  248,  248,  247,  247,  247,  247,
    0,    0,    0,    0,  248,    0,    0,  248,  248,  248,
  248,  248,  248,  248,  248,  248,  248,    0,  248,  248,
    0,  248,  248,  248,  248,  248,  248,  248,  249,  249,
  248,  248,  248,  248,    0,   69,  248,    0,    0,    0,
  248,  248,  248,  248,  248,    0,  248,  248,  248,  248,
  248,  248,  248,    0,  248,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  248,    0,    0,    0,    0,
    0,    0,    0,  249,  249,  249,  248,  248,  248,  248,
    0,    0,    0,    0,  249,    0,    0,  249,  249,  249,
  249,  249,  249,  249,  249,  249,  249,    0,  249,  249,
    0,  249,  249,  249,  249,  249,  249,  249,  280,  280,
  249,  249,  249,  249,    0,   69,  249,    0,    0,    0,
  249,  249,  249,  249,  249,    0,  249,  249,  249,  249,
  249,  249,  249,    0,  249,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  249,    0,    0,    0,    0,
    0,    0,    0,  280,  280,  280,  249,  249,  249,  249,
    0,    0,    0,    0,  280,    0,    0,  280,  280,  280,
  280,  280,  280,  280,  280,  280,  280,    0,  280,  280,
    0,  280,  280,  280,  280,  280,  280,  280,  272,  272,
  280,  280,  280,  280,    0,   69,  280,    0,    0,    0,
  280,  280,  280,  280,  280,    0,  280,  280,  280,  280,
  280,  280,  280,    0,  280,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  280,    0,    0,    0,    0,
    0,    0,    0,  272,  272,  272,  280,  280,  280,  280,
    0,    0,    0,    0,  272,    0,    0,  272,  272,  272,
  272,  272,  272,  272,  272,  272,  272,    0,  272,  272,
    0,  272,  272,  272,  272,  272,  272,  272,  265,  265,
  272,  272,  272,  272,    0,    0,  272,    0,    0,    0,
  272,  272,  272,  272,  272,    0,  272,  272,  272,  272,
  272,  272,  272,    0,  272,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  272,    0,    0,    0,    0,
    0,    0,    0,  265,  265,  265,  272,  272,  272,  272,
    0,    0,    0,   66,  265,    0,    0,  265,  265,  265,
  265,  265,  265,  265,  265,  265,  265,    0,  265,  265,
    0,  265,  265,  265,  265,  265,  265,  265,  256,  256,
  265,  265,  265,  265,    0,   69,  265,    0,    0,    0,
  265,  265,  265,  265,  265,    0,  265,  265,  265,  265,
  265,  265,  265,    0,  265,   69,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  265,    0,    0,    0,    0,
    0,    0,    0,  256,  256,  256,  265,  265,  265,  265,
    0,    0,    0,   66,  256,    0,   69,  256,  256,  256,
  256,  256,  256,  256,  256,  256,  256,    0,  256,  256,
    0,  256,  256,  256,  256,  256,  256,  256,  232,  232,
  256,  256,  256,  256,    0,    0,  256,   66,   69,    0,
  256,  256,  256,  256,  256,    0,  256,  256,  256,  256,
  256,  256,  256,    0,  256,  132,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  256,    0,    0,    0,    0,
    0,    0,    0,  232,  232,  232,  256,  256,  256,  256,
    0,    0,    0,    0,  232,    0,  133,  232,  232,  232,
  232,  232,  232,  232,  232,  232,  232,    0,  232,  232,
    0,  232,  232,  232,  232,  232,  232,  232,    0,    0,
  232,  232,  232,  232,    0,   66,  232,  247,  131,    0,
  232,  232,  232,  232,  232,    0,  232,  232,  232,  232,
  232,  232,  232,  132,  232,    0,    0,    0,  116,  117,
  118,    0,  119,  120,  121,  232,  122,    0,    0,    0,
    0,    0,    0,  729,  730,    0,  232,  232,  232,  232,
  125,   66,    0,    0,  133,    0,    0,  126,    0,    0,
    0,    0,   69,   69,   69,    0,   69,   69,   69,  132,
   69,    0,    0,    0,    0,    0,    0,   69,   69,    0,
    0,    0,    0,    0,   69,    0,  131,    0,  132,    0,
    0,   69,    0,    0,    0,    0,    0,    0,   32,    0,
  133,    0,    0,    0,    0,    0,    0,    0,  116,  117,
  118,  263,  119,  120,  121,    0,  122,    0,  269,  133,
    0,    0,    0,    0,    0,    0,    0,    0,    0,   33,
  125,    0,  131,    0,    0,    0,    0,  126,    0,    0,
    0,    0,  116,  117,  118,  132,  119,  120,  121,    0,
  122,  131,    0,    0,  291,  292,    0,  123,  124,    0,
    0,   31,    0,    0,  125,    0,   69,    0,    0,   32,
    0,  126,    0,    0,    0,    0,  133,  359,    0,    0,
   69,   69,   69,  164,    0,   96,   97,    0,   98,   99,
  100,  101,  102,  103,    0,    0,    0,    0,  109,    0,
   33,    0,   32,    0,    0,    0,    0,    0,  131,    0,
    0,    0,    0,    0,    0,    0,    0,   32,    0,    0,
  116,  117,  118,    0,  119,  120,  121,    0,  122,    0,
    0,    0,   31,   33,    0,  123,  124,  463,    0,  464,
    0,  467,  125,    0,    0,    0,  127,    0,   33,  126,
    0,    0,    0,    0,  477,    0,  479,    0,    0,  459,
  128,  129,  130,    0,    0,   31,  116,  117,  118,    0,
  119,  120,  121,    0,  122,    0,    0,    0,    0,    0,
   31,  123,  124,    0,    0,  116,  117,  118,  125,  119,
  120,  121,    0,  122,    0,  126,    0,    0,    0,    0,
  123,  124,   21,    0,    0,    0,    0,  125,    0,    0,
    0,   22,    0,    0,  126,    0,    0,    0,    0,   23,
   24,   25,   26,   27,  127,    0,    0,    0,   96,   97,
    0,   98,   99,  100,  101,  102,  103,    0,  128,  129,
  130,    0,  116,  117,  118,    0,  119,  120,  121,  469,
  122,    0,    0,    0,    0,  547,    0,  123,  124,    0,
    0,    0,  551,    0,  125,    0,    0,    0,  376,  377,
  127,  126,  554,   21,  555,    0,    0,    0,    0,  558,
    0,    0,   22,    0,  128,  129,  130,    0,    0,  127,
   23,   24,   25,   26,   27,    0,    0,    0,    0,    0,
    0,    0,    0,  128,  129,  130,   21,  293,    0,    0,
    0,  406,    0,    0,    0,   22,    0,    0,    0,    0,
  162,   21,    0,   23,   24,   25,   26,   27,    0,  471,
   22,    0,    0,    0,    0,  163,    0,    0,   23,   24,
   25,   26,   27,    0,    0,    0,  127,    0,    0,    0,
  108,    0,  294,  295,  296,    0,    0,    0,    0,    0,
  128,  129,  130,  297,    0,    0,  298,  299,  300,  301,
  302,  303,  304,  305,  306,  307,    0,  308,  309,    0,
  310,  311,  312,  313,  314,  315,  316,    0,    0,  317,
  318,  319,  320,    0,    0,  321,    0,  293,    0,  322,
  323,  324,  325,  326,    0,  327,  328,  329,  330,  331,
  332,  333,    0,  334,    0,    0,  482,    0,    0,    0,
    0,    0,    0,    0,  335,    0,  116,  117,  118,    0,
  119,  120,  121,    0,  122,  336,  337,  338,  339,    0,
    0,    0,  294,  295,  296,  274,    0,    0,  125,    0,
    0,    0,    0,  297,    0,  126,  298,  299,  300,  301,
  302,  303,  304,  305,  306,  307,    0,  308,  309,    0,
  310,  311,  312,  313,  314,  315,  316,  293,    0,  317,
  318,  319,  320,    0,    0,  321,    0,    0,    0,  322,
  323,  324,  325,  326,    0,  327,  328,  329,  330,  331,
  332,  333,    0,  334,    0,  548,    0,    0,    0,    0,
    0,    0,    0,    0,  335,    0,    0,    0,    0,    0,
    0,    0,  294,  295,  296,  336,  337,  338,  339,    0,
    0,    0,    0,  297,    0,    0,  298,  299,  300,  301,
  302,  303,  304,  305,  306,  307,    0,  308,  309,    0,
  310,  311,  312,  313,  314,  315,  316,    0,    0,  317,
  318,  319,  320,    0,  293,  321,    0,    0,    0,  322,
  323,  324,  325,  326,    0,  327,  328,  329,  330,  331,
  332,  333,    0,  334,    0,  552,    0,    0,    0,    0,
    0,    0,    0,    0,  335,  426,  427,  428,  429,  430,
  431,  432,  433,  434,  435,  336,  337,  338,  339,  294,
  295,  296,    0,    0,    0,    0,    0,    0,    0,    0,
  297,    0,    0,  298,  299,  300,  301,  302,  303,  304,
  305,  306,  307,    0,  308,  309,    0,  310,  311,  312,
  313,  314,  315,  316,    0,    0,  317,  318,  319,  320,
    0,    0,  321,  293,    0,    0,  322,  323,  324,  325,
  326,    0,  327,  328,  329,  330,  331,  332,  333,    0,
  334,    0,  556,    0,    0,    0,    0,    0,    0,    0,
    0,  335,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  336,  337,  338,  339,    0,    0,  294,  295,
  296,    0,    0,    0,    0,    0,    0,    0,    0,  297,
    0,    0,  298,  299,  300,  301,  302,  303,  304,  305,
  306,  307,    0,  308,  309,    0,  310,  311,  312,  313,
  314,  315,  316,  293,    0,  317,  318,  319,  320,    0,
    0,  321,    0,    0,    0,  322,  323,  324,  325,  326,
    0,  327,  328,  329,  330,  331,  332,  333,    0,  334,
    0,  557,    0,    0,    0,    0,    0,    0,    0,    0,
  335,    0,    0,    0,    0,    0,    0,    0,  294,  295,
  296,  336,  337,  338,  339,    0,    0,    0,    0,  297,
    0,    0,  298,  299,  300,  301,  302,  303,  304,  305,
  306,  307,    0,  308,  309,    0,  310,  311,  312,  313,
  314,  315,  316,    0,    0,  317,  318,  319,  320,    0,
  293,  321,    0,    0,    0,  322,  323,  324,  325,  326,
    0,  327,  328,  329,  330,  331,  332,  333,    0,  334,
    0,  619,    0,    0,    0,    0,    0,    0,    0,    0,
  335,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  336,  337,  338,  339,  294,  295,  296,    0,    0,
    0,    0,    0,    0,    0,    0,  297,    0,    0,  298,
  299,  300,  301,  302,  303,  304,  305,  306,  307,    0,
  308,  309,    0,  310,  311,  312,  313,  314,  315,  316,
    0,    0,  317,  318,  319,  320,    0,    0,  321,  293,
    0,    0,  322,  323,  324,  325,  326,    0,  327,  328,
  329,  330,  331,  332,  333,    0,  334,    0,  677,    0,
    0,    0,    0,    0,    0,    0,    0,  335,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  336,  337,
  338,  339,    0,    0,  294,  295,  296,    0,    0,    0,
    0,    0,    0,    0,    0,  297,    0,    0,  298,  299,
  300,  301,  302,  303,  304,  305,  306,  307,    0,  308,
  309,    0,  310,  311,  312,  313,  314,  315,  316,  293,
    0,  317,  318,  319,  320,    0,    0,  321,    0,    0,
    0,  322,  323,  324,  325,  326,    0,  327,  328,  329,
  330,  331,  332,  333,    0,  334,    0,  714,    0,    0,
    0,    0,    0,    0,    0,    0,  335,    0,    0,    0,
    0,    0,    0,    0,  294,  295,  296,  336,  337,  338,
  339,    0,    0,    0,    0,  297,    0,    0,  298,  299,
  300,  301,  302,  303,  304,  305,  306,  307,    0,  308,
  309,    0,  310,  311,  312,  313,  314,  315,  316,    0,
    0,  317,  318,  319,  320,    0,  293,  321,    0,    0,
    0,  322,  323,  324,  325,  326,    0,  327,  328,  329,
  330,  331,  332,  333,    0,  334,    0,  184,    0,    0,
    0,    0,    0,    0,    0,    0,  335,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  336,  337,  338,
  339,  294,  295,  296,    0,    0,    0,    0,    0,    0,
    0,    0,  297,    0,    0,  298,  299,  300,  301,  302,
  303,  304,  305,  306,  307,    0,  308,  309,    0,  310,
  311,  312,  313,  314,  315,  316,    0,    0,  317,  318,
  319,  320,    0,    0,  321,  293,    0,    0,  322,  323,
  324,  325,  326,    0,  327,  328,  329,  330,  331,  332,
  333,    0,  334,    0,  182,    0,    0,    0,    0,    0,
    0,    0,    0,  335,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  336,  337,  338,  339,    0,    0,
  294,  295,  296,    0,    0,    0,    0,    0,    0,    0,
    0,  297,    0,    0,  298,  299,  300,  301,  302,  303,
  304,  305,  306,  307,    0,  308,  309,    0,  310,  311,
  312,  313,  314,  315,  316,  184,    0,  317,  318,  319,
  320,    0,    0,  321,    0,    0,    0,  322,  323,  324,
  325,  326,    0,  327,  328,  329,  330,  331,  332,  333,
    0,  334,    0,  185,    0,    0,    0,    0,    0,    0,
    0,    0,  335,    0,    0,    0,    0,    0,    0,    0,
  184,  184,  184,  336,  337,  338,  339,    0,    0,    0,
    0,  184,    0,    0,  184,  184,  184,  184,  184,  184,
  184,  184,  184,  184,    0,  184,  184,    0,  184,  184,
  184,  184,  184,  184,  184,    0,    0,  184,  184,  184,
  184,    0,  182,  184,    0,    0,    0,  184,  184,  184,
  184,  184,    0,  184,  184,  184,  184,  184,  184,  184,
    0,  184,    0,  183,    0,    0,    0,    0,    0,    0,
    0,    0,  184,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  184,  184,  184,  184,  182,  182,  182,
    0,    0,    0,    0,    0,    0,    0,    0,  182,    0,
    0,  182,  182,  182,  182,  182,  182,  182,  182,  182,
  182,    0,  182,  182,    0,  182,  182,  182,  182,  182,
  182,  182,   32,    0,  182,  182,  182,  182,    0,    0,
  182,  185,    0,    0,  182,  182,  182,  182,  182,    0,
  182,  182,  182,  182,  182,  182,  182,    0,  182,    0,
    0,    0,    0,   33,    0,    0,    0,    0,    0,  182,
    0,    0,  667,    0,    0,    0,    0,    0,    0,    0,
  182,  182,  182,  182,    0,    0,  185,  185,  185,    0,
    0,   32,    0,    0,    0,   31,    0,  185,    0,    0,
  185,  185,  185,  185,  185,  185,  185,  185,  185,  185,
    0,  185,  185,    0,  185,  185,  185,  185,  185,  185,
  185,  183,   33,  185,  185,  185,  185,    0,    0,  185,
   32,    0,    0,  185,  185,  185,  185,  185,    0,  185,
  185,  185,  185,  185,  185,  185,    0,  185,    0,    0,
    0,    0,    0,    0,   31,    0,    0,    0,  185,    0,
    0,   33,    0,    0,    0,    0,  183,  183,  183,  185,
  185,  185,  185,    0,    0,    0,   32,  183,    0,    0,
  183,  183,  183,  183,  183,  183,  183,  183,  183,  183,
    0,  183,  183,   31,  183,  183,  183,  183,  183,  183,
  183,   32,    0,  183,  183,  183,  183,   33,    0,  183,
    0,    0,    0,  183,  183,  183,  183,  183,    0,  183,
  183,  183,  183,  183,  183,  183,    0,  183,   32,    0,
    0,    0,   33,    0,    0,    0,   21,    0,  183,   31,
    0,    0,    0,   32,    0,   22,    0,    0,    0,  183,
  183,  183,  183,   23,   24,   25,   26,   27,   32,   33,
    0,    0,    0,    0,   31,    0,    0,    0,    0,    0,
    0,    0,   96,   97,   33,   98,   99,  100,  101,  102,
  103,    0,    0,    0,    0,    0,    0,   32,    0,   33,
    0,   31,    0,   32,    0,   21,    0,    0,    0,    0,
   32,    0,    0,    0,   22,    0,   31,    0,    0,  666,
    0,    0,   23,   24,   25,   26,   27,    0,   33,    0,
    0,   31,    0,    0,   33,    0,    0,    0,   32,    0,
    0,   33,    0,   32,  146,    0,    0,    0,    0,    0,
    0,    0,    0,   22,    0,    0,    0,    0,    0,    0,
   31,   23,   24,   25,   26,   27,   31,   32,  149,   33,
    0,    0,    0,   31,   33,   59,    0,    0,    0,    0,
    0,   32,  408,    0,    0,    0,    0,    0,    0,    0,
   21,    0,    0,    0,    0,    0,    0,    0,   33,   22,
  719,   31,    0,    0,    0,    0,   31,   23,   24,   25,
   26,   27,   33,    0,    0,   21,   32,    0,    0,    0,
    0,   32,    0,    0,   22,   67,    0,    0,    0,    0,
   31,    0,   23,   24,   25,   26,   27,    0,    0,    0,
    0,    0,   21,   28,   31,    0,    0,   33,   29,   30,
    0,   22,   33,    0,    0,    0,  162,   21,    0,   23,
   24,   25,   26,   27,    0,   60,   22,    0,    0,    0,
    0,  163,   21,    0,   23,   24,   25,   26,   27,   31,
   61,   22,    0,    0,   31,    0,  108,    0,    0,   23,
   24,   25,   26,   27,    0,    0,    0,    0,    0,    0,
   38,  146,  147,    0,    0,    0,    0,  146,  147,    0,
   22,  148,    0,    0,   21,    0,   22,  148,   23,   24,
   25,   26,   27,   22,   23,   24,   25,   26,   27,    0,
    0,   23,   24,   25,   26,   27,    0,    0,    0,    0,
    0,    0,  146,  257,    0,    0,    0,   21,    0,    0,
    0,   22,  258,  437,  438,    0,   22,  704,    0,   23,
   24,   25,   26,   27,   23,   24,   25,   26,   27,    0,
    0,   21,    0,    0,    0,    0,    0,    0,    0,    0,
   22,    0,    0,    0,    0,   21,    0,    0,   23,   24,
   25,   26,   27,    0,   22,    0,    0,    0,    0,  666,
   60,    0,   23,   24,   25,   26,   27,    0,    0,    0,
    0,    0,    0,    0,    0,   61,   60,    0,    0,    0,
   21,    0,    0,    0,    0,  146,    0,    0,    0,   22,
    0,   61,    0,    0,   22,    0,    0,   23,   24,   25,
   26,   27,   23,   24,   25,   26,   27,   60,   60,  264,
  264,   60,   60,   60,   60,   60,   60,    0,    0,    0,
    0,    0,   61,   61,    0,    0,   61,   61,   61,   61,
   61,   61,    0,    0,  439,  440,  441,  442,    0,    0,
    0,    0,    0,  443,  444,  445,  446,  447,  448,  449,
  450,  451,  452,    0,  264,  264,  264,    0,    0,    0,
    0,    0,    0,    0,    0,  264,    0,    0,  264,  264,
  264,  264,  264,  264,  264,  264,  264,  264,    0,  264,
  264,    0,  264,  264,  264,  264,  264,  264,  264,  233,
  233,  264,  264,  264,  264,    0,    0,  264,    0,    0,
    0,  264,  264,  264,  264,  264,    0,  264,  264,  264,
  264,  264,  264,  264,    0,  264,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  264,    0,    0,    0,
    0,    0,    0,    0,  233,  233,  233,  264,  264,  264,
  264,    0,    0,    0,    0,  233,    0,    0,  233,  233,
  233,  233,  233,  233,  233,  233,  233,  233,    0,  233,
  233,    0,  233,  233,  233,  233,  233,  233,  233,  236,
  236,  233,  233,  233,  233,    0,    0,  233,    0,    0,
    0,  233,  233,  233,  233,  233,    0,  233,  233,  233,
  233,  233,  233,  233,    0,  233,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  233,    0,    0,    0,
    0,    0,    0,    0,  236,  236,  236,  233,  233,  233,
  233,    0,    0,    0,    0,  236,    0,    0,  236,  236,
  236,  236,  236,  236,  236,  236,  236,  236,    0,  236,
  236,    0,  236,  236,  236,  236,  236,  236,  236,  238,
  238,  236,  236,  236,  236,    0,    0,  236,    0,    0,
    0,  236,  236,  236,  236,  236,    0,  236,  236,  236,
  236,  236,  236,  236,    0,  236,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  236,    0,    0,    0,
    0,    0,    0,    0,  238,  238,  238,  236,  236,  236,
  236,    0,    0,    0,    0,  238,    0,    0,  238,  238,
  238,  238,  238,  238,  238,  238,  238,  238,    0,  238,
  238,    0,  238,  238,  238,  238,  238,  238,  238,  252,
  252,  238,  238,  238,  238,    0,    0,  238,    0,    0,
    0,  238,  238,  238,  238,  238,    0,  238,  238,  238,
  238,  238,  238,  238,    0,  238,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  238,    0,    0,    0,
    0,    0,    0,    0,  252,  252,  252,  238,  238,  238,
  238,    0,    0,    0,    0,  252,    0,    0,  252,  252,
  252,  252,  252,  252,  252,  252,  252,  252,    0,  252,
  252,    0,  252,  252,  252,  252,  252,  252,  252,  237,
  237,  252,  252,  252,  252,    0,    0,  252,    0,    0,
    0,  252,  252,  252,  252,  252,    0,  252,  252,  252,
  252,  252,  252,  252,    0,  252,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  252,    0,    0,    0,
    0,    0,    0,    0,  237,  237,  237,  252,  252,  252,
  252,    0,    0,    0,    0,  237,    0,    0,  237,  237,
  237,  237,  237,  237,  237,  237,  237,  237,    0,  237,
  237,    0,  237,  237,  237,  237,  237,  237,  237,  253,
  253,  237,  237,  237,  237,    0,    0,  237,    0,    0,
    0,  237,  237,  237,  237,  237,    0,  237,  237,  237,
  237,  237,  237,  237,    0,  237,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  237,    0,    0,    0,
    0,    0,    0,    0,  253,  253,  253,  237,  237,  237,
  237,    0,    0,    0,    0,  253,    0,    0,  253,  253,
  253,  253,  253,  253,  253,  253,  253,  253,    0,  253,
  253,    0,  253,  253,  253,  253,  253,  253,  253,  293,
    0,  253,  253,  253,  253,    0,    0,  253,    0,    0,
    0,  253,  253,  253,  253,  253,    0,  253,  253,  253,
  253,  253,  253,  253,    0,  253,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  253,    0,    0,    0,
    0,    0,    0,    0,  294,  295,  296,  253,  253,  253,
  253,    0,    0,    0,    0,  297,    0,    0,  298,  299,
  300,  301,  302,  303,  304,  305,  306,  307,    0,  308,
  309,    0,  310,  311,  312,  313,  314,  315,  316,  188,
    0,  317,  318,  319,  320,    0,    0,  321,    0,    0,
    0,  322,  323,  324,  325,  326,    0,  327,  328,  329,
  330,  331,  332,  333,    0,  334,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  335,    0,    0,    0,
    0,    0,    0,    0,  188,  188,  188,  336,  337,  338,
  339,    0,    0,    0,    0,  188,    0,    0,  188,  188,
  188,  188,  188,  188,  188,  188,  188,  188,    0,  188,
  188,    0,  188,  188,  188,  188,  188,  188,  188,  189,
    0,  188,  188,  188,  188,    0,    0,  188,    0,    0,
    0,  188,  188,  188,  188,  188,    0,  188,  188,  188,
  188,  188,  188,  188,    0,  188,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  188,    0,    0,    0,
    0,    0,    0,    0,  189,  189,  189,  188,  188,  188,
  188,    0,    0,    0,    0,  189,    0,    0,  189,  189,
  189,  189,  189,  189,  189,  189,  189,  189,    0,  189,
  189,    0,  189,  189,  189,  189,  189,  189,  189,  190,
    0,  189,  189,  189,  189,    0,    0,  189,    0,    0,
    0,  189,  189,  189,  189,  189,    0,  189,  189,  189,
  189,  189,  189,  189,    0,  189,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  189,    0,    0,    0,
    0,    0,    0,    0,  190,  190,  190,  189,  189,  189,
  189,    0,    0,    0,    0,  190,    0,    0,  190,  190,
  190,  190,  190,  190,  190,  190,  190,  190,    0,  190,
  190,    0,  190,  190,  190,  190,  190,  190,  190,  191,
    0,  190,  190,  190,  190,    0,    0,  190,    0,    0,
    0,  190,  190,  190,  190,  190,    0,  190,  190,  190,
  190,  190,  190,  190,    0,  190,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  190,    0,    0,    0,
    0,    0,    0,    0,  191,  191,  191,  190,  190,  190,
  190,    0,    0,    0,    0,  191,    0,    0,  191,  191,
  191,  191,  191,  191,  191,  191,  191,  191,    0,  191,
  191,    0,  191,  191,  191,  191,  191,  191,  191,    0,
    0,  191,  191,  191,  191,    0,    0,  191,    0,    0,
    0,  191,  191,  191,  191,  191,    0,  191,  191,  191,
  191,  191,  191,  191,    0,  191,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  191,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  191,  191,  191,
  191,  297,    0,    0,  298,  299,  300,  301,  302,  303,
  304,  305,  306,  307,    0,  308,  309,    0,  310,  311,
  312,  313,  314,  315,  316,    0,    0,  317,  318,  319,
  320,    0,    0,  321,    0,    0,    0,  322,  323,  324,
  325,  326,    0,  327,  328,  329,  330,  331,  332,  333,
    0,  334,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  335,  116,  117,  118,  249,  119,  120,  121,
    0,  122,    0,  336,  337,  338,  339,    0,    0,    0,
  250,    0,  251,    0,    0,  125,    0,    0,    0,    0,
    0,    0,  126,
  };
  protected static readonly short [] yyCheck = {             6,
    7,   69,  202,    6,    7,  289,   33,  488,  114,  123,
  123,   60,   33,  123,   40,   44,  123,  338,  123,  123,
  123,   28,   40,   40,   31,   28,  123,   60,   40,   36,
  123,   38,  232,   36,  170,   38,   40,  123,  268,  123,
  176,   41,  272,   50,   42,   44,  123,   44,   40,   40,
   44,  274,  614,  159,  616,   44,   44,   64,   91,   42,
  290,   40,   69,  169,   71,   44,   69,  309,  174,  175,
  543,   78,  545,   80,  123,  276,  660,  520,  559,  280,
   42,  217,   44,   42,   91,   92,  309,   94,  224,  289,
  123,  270,  271,  229,  230,   93,  125,   40,  105,  106,
  340,   44,  105,   62,  266,   93,  212,   61,   91,  349,
  216,  673,  352,  675,  698,   44,  346,  294,  295,   88,
  350,  290,  291,  285,  260,  261,  125,  367,  125,   61,
  137,  125,  139,   62,   61,    0,  125,  458,  368,  369,
  309,   61,  615,   61,  617,  588,  125,  257,  591,  260,
   51,  713,   53,  221,  354,   40,   41,   61,  264,   44,
  274,  274,  131,  132,  133,  280,  173,  274,  379,  274,
  274,  274,  383,  290,  291,  386,  145,  274,   79,  257,
   81,  274,  125,  394,    0,   40,   41,   61,  274,   44,
  274,   42,    0,   44,   40,  309,  309,  274,   42,  299,
  681,  301,    0,  676,  309,  309,   41,   41,  308,   44,
   44,  347,   41,  260,   60,   44,  223,   41,   41,   42,
   44,  190,  191,  192,   41,  465,   41,   44,  235,   44,
  470,  264,   41,  132,  133,   44,  466,  718,  287,   40,
  273,  210,  348,  483,  484,   91,  272,  274,  281,  282,
  283,  284,  285,  274,  272,  272,  277,  257,  258,  259,
  272,  261,  262,  263,  260,  265,   52,   53,  272,   41,
   42,  240,  279,  280,  281,  274,  276,  123,   77,  279,
  272,  272,   41,   42,   41,  274,  286,   44,    0,   40,
  297,  298,  299,  300,  301,  302,  303,  304,  305,  306,
  307,  308,  309,  310,  311,  312,  313,  267,   40,  549,
  317,  318,  345,   41,  321,   42,   44,   44,  290,  291,
  550,  107,  294,  295,   42,  294,   44,  296,  114,  336,
  337,  338,  468,   41,  123,  338,   44,  136,   41,  138,
   41,   44,   42,   44,   44,  314,  315,  316,    0,   42,
  319,   44,  151,  322,  323,  324,  325,  326,  327,  328,
  329,  330,  331,  332,  333,  267,   40,  123,  154,   42,
  123,  123,  379,  159,   40,   40,  383,    0,  363,  386,
  620,   40,   44,   58,   62,  309,  355,  394,  395,  457,
  397,  621,   61,   40,   40,   58,  362,  274,  274,  406,
  199,    0,  201,  268,  269,  412,   44,  272,  273,   73,
  275,  257,  258,  259,   44,  261,  262,  263,  260,  265,
  370,  370,  370,  288,  289,   89,  272,  273,  362,  436,
  257,   95,  218,  279,   44,   44,  362,  362,  678,  408,
  286,  274,   40,  274,  728,  310,  453,  257,  362,  113,
  457,  458,  268,  269,  457,  458,  272,  273,   61,  275,
  268,  269,  257,  257,  272,  273,  362,  275,  317,  274,
  268,  269,  288,  289,  272,  273,  257,  275,  546,  265,
  288,  289,  682,  257,  403,  273,   44,   44,   44,   44,
  288,  289,   44,  266,  310,  370,  160,  370,  370,  370,
  370,  370,  310,  370,  370,  705,  370,  613,  370,  370,
  370,  518,  310,  520,   44,  361,  523,  524,  525,  526,
  527,  528,  529,  530,  531,  532,  533,  534,  728,  375,
  376,  377,    0,   44,   44,  671,   44,  544,   44,  546,
   44,  544,    0,  546,  513,  514,  515,   44,   44,   44,
   44,   44,   44,  522,   44,  219,  268,  269,   44,   44,
  272,  273,   44,  275,   44,   44,  125,   44,  674,  362,
   44,   41,   44,   44,  373,  711,  288,  289,   40,  378,
   44,  588,  381,  382,  591,  384,  385,   44,  387,  388,
  389,  390,  391,  392,  393,   44,   44,  396,  310,  398,
  399,  400,  401,   91,   44,  612,   44,   44,   44,  257,
   44,  618,   44,   44,   44,  618,  268,  269,   44,  626,
  272,  273,   44,  275,  593,   40,   41,  300,  301,   44,
  303,  304,  305,  306,  307,  308,  288,  289,   91,  707,
   44,   44,  611,  362,   44,  268,  269,   62,  362,  272,
  273,   44,  275,   44,   44,  362,  455,  257,  310,  666,
  362,  257,   60,   44,   93,  288,  289,   40,    0,  268,
  269,  257,  257,  272,  273,   40,  275,  684,   93,  268,
  269,  650,  651,  272,  273,   10,  275,  310,   60,  288,
  289,  490,   54,   91,  145,  494,   18,  144,  497,  288,
  289,  260,  709,  205,  187,   40,  505,  506,   64,  508,
  125,  310,  173,  668,  223,  680,  342,  276,  342,   91,
  370,  310,  709,  300,  301,  123,  303,  304,  305,  306,
  307,  308,  354,  684,  703,   -1,  535,  536,  537,  290,
  291,   -1,  610,  294,  295,  296,  297,  298,  307,  308,
   -1,  123,  311,  312,  313,  314,  315,  316,   -1,   -1,
   -1,   -1,  561,   -1,  563,  564,   -1,  566,  567,   -1,
  569,  570,  571,  572,  573,  574,  575,   -1,   -1,  578,
   -1,  580,  581,  582,  583,   40,   -1,  257,  258,  259,
  589,  261,  262,  263,   -1,  265,   -1,   -1,   -1,   -1,
  268,  269,   -1,   -1,  272,  273,  276,  275,   -1,  279,
  268,  269,   -1,   -1,  272,  273,  286,  275,   -1,   -1,
  288,  289,   -1,   -1,   -1,  260,   -1,   -1,   -1,  628,
  288,  289,  631,   -1,   -1,  634,   -1,   -1,   -1,   -1,
   -1,  276,  310,  642,  643,   -1,  645,   -1,   -1,   -1,
   -1,   -1,  310,   -1,   -1,   -1,   -1,   -1,  273,  274,
   -1,   -1,  661,  662,  663,   40,  264,   -1,   -1,  668,
   -1,   -1,  307,  308,   -1,  273,  311,  312,  313,  314,
  315,  316,   -1,  281,  282,  283,  284,  285,   -1,   -1,
   -1,   -1,  264,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  273,   -1,  318,  319,  320,   -1,   -1,  707,  281,
  282,  283,  284,  285,  329,   -1,   -1,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   40,  361,   -1,   -1,   -1,
  365,  366,  367,  368,  369,  370,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,  363,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  345,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   40,  361,   -1,   -1,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   40,  361,   -1,   -1,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   40,  361,   -1,   -1,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   40,  361,   -1,   -1,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   40,  361,   -1,   -1,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   40,  361,   -1,   -1,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   40,  361,   -1,   -1,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   40,  361,   -1,   -1,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   42,  329,   -1,   -1,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   40,  361,   -1,   -1,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,   60,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   42,  329,   -1,   91,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,  273,  274,
  355,  356,  357,  358,   -1,   -1,  361,   42,  123,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,   60,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,  404,
   -1,   -1,   -1,   -1,  329,   -1,   91,  332,  333,  334,
  335,  336,  337,  338,  339,  340,  341,   -1,  343,  344,
   -1,  346,  347,  348,  349,  350,  351,  352,   -1,   -1,
  355,  356,  357,  358,   -1,   42,  361,   44,  123,   -1,
  365,  366,  367,  368,  369,   -1,  371,  372,  373,  374,
  375,  376,  377,   60,  379,   -1,   -1,   -1,  257,  258,
  259,   -1,  261,  262,  263,  390,  265,   -1,   -1,   -1,
   -1,   -1,   -1,  272,  273,   -1,  401,  402,  403,  404,
  279,   42,   -1,   -1,   91,   -1,   -1,  286,   -1,   -1,
   -1,   -1,  257,  258,  259,   -1,  261,  262,  263,   60,
  265,   -1,   -1,   -1,   -1,   -1,   -1,  272,  273,   -1,
   -1,   -1,   -1,   -1,  279,   -1,  123,   -1,   60,   -1,
   -1,  286,   -1,   -1,   -1,   -1,   -1,   -1,   60,   -1,
   91,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  257,  258,
  259,  217,  261,  262,  263,   -1,  265,   -1,  224,   91,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   91,
  279,   -1,  123,   -1,   -1,   -1,   -1,  286,   -1,   -1,
   -1,   -1,  257,  258,  259,   60,  261,  262,  263,   -1,
  265,  123,   -1,   -1,  260,  261,   -1,  272,  273,   -1,
   -1,  123,   -1,   -1,  279,   -1,  361,   -1,   -1,   60,
   -1,  286,   -1,   -1,   -1,   -1,   91,  283,   -1,   -1,
  375,  376,  377,   41,   -1,  300,  301,   -1,  303,  304,
  305,  306,  307,  308,   -1,   -1,   -1,   -1,   41,   -1,
   91,   -1,   60,   -1,   -1,   -1,   -1,   -1,  123,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   60,   -1,   -1,
  257,  258,  259,   -1,  261,  262,  263,   -1,  265,   -1,
   -1,   -1,  123,   91,   -1,  272,  273,  343,   -1,  345,
   -1,  347,  279,   -1,   -1,   -1,  361,   -1,   91,  286,
   -1,   -1,   -1,   -1,  360,   -1,  362,   -1,   -1,  125,
  375,  376,  377,   -1,   -1,  123,  257,  258,  259,   -1,
  261,  262,  263,   -1,  265,   -1,   -1,   -1,   -1,   -1,
  123,  272,  273,   -1,   -1,  257,  258,  259,  279,  261,
  262,  263,   -1,  265,   -1,  286,   -1,   -1,   -1,   -1,
  272,  273,  264,   -1,   -1,   -1,   -1,  279,   -1,   -1,
   -1,  273,   -1,   -1,  286,   -1,   -1,   -1,   -1,  281,
  282,  283,  284,  285,  361,   -1,   -1,   -1,  300,  301,
   -1,  303,  304,  305,  306,  307,  308,   -1,  375,  376,
  377,   -1,  257,  258,  259,   -1,  261,  262,  263,  125,
  265,   -1,   -1,   -1,   -1,  461,   -1,  272,  273,   -1,
   -1,   -1,  468,   -1,  279,   -1,   -1,   -1,  330,  331,
  361,  286,  478,  264,  480,   -1,   -1,   -1,   -1,  485,
   -1,   -1,  273,   -1,  375,  376,  377,   -1,   -1,  361,
  281,  282,  283,  284,  285,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  375,  376,  377,  264,  273,   -1,   -1,
   -1,  302,   -1,   -1,   -1,  273,   -1,   -1,   -1,   -1,
  278,  264,   -1,  281,  282,  283,  284,  285,   -1,  125,
  273,   -1,   -1,   -1,   -1,  293,   -1,   -1,  281,  282,
  283,  284,  285,   -1,   -1,   -1,  361,   -1,   -1,   -1,
  293,   -1,  318,  319,  320,   -1,   -1,   -1,   -1,   -1,
  375,  376,  377,  329,   -1,   -1,  332,  333,  334,  335,
  336,  337,  338,  339,  340,  341,   -1,  343,  344,   -1,
  346,  347,  348,  349,  350,  351,  352,   -1,   -1,  355,
  356,  357,  358,   -1,   -1,  361,   -1,  273,   -1,  365,
  366,  367,  368,  369,   -1,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,   -1,   -1,  125,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  390,   -1,  257,  258,  259,   -1,
  261,  262,  263,   -1,  265,  401,  402,  403,  404,   -1,
   -1,   -1,  318,  319,  320,  276,   -1,   -1,  279,   -1,
   -1,   -1,   -1,  329,   -1,  286,  332,  333,  334,  335,
  336,  337,  338,  339,  340,  341,   -1,  343,  344,   -1,
  346,  347,  348,  349,  350,  351,  352,  273,   -1,  355,
  356,  357,  358,   -1,   -1,  361,   -1,   -1,   -1,  365,
  366,  367,  368,  369,   -1,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,   -1,  125,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  318,  319,  320,  401,  402,  403,  404,   -1,
   -1,   -1,   -1,  329,   -1,   -1,  332,  333,  334,  335,
  336,  337,  338,  339,  340,  341,   -1,  343,  344,   -1,
  346,  347,  348,  349,  350,  351,  352,   -1,   -1,  355,
  356,  357,  358,   -1,  273,  361,   -1,   -1,   -1,  365,
  366,  367,  368,  369,   -1,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,   -1,  125,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  390,  380,  381,  382,  383,  384,
  385,  386,  387,  388,  389,  401,  402,  403,  404,  318,
  319,  320,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  329,   -1,   -1,  332,  333,  334,  335,  336,  337,  338,
  339,  340,  341,   -1,  343,  344,   -1,  346,  347,  348,
  349,  350,  351,  352,   -1,   -1,  355,  356,  357,  358,
   -1,   -1,  361,  273,   -1,   -1,  365,  366,  367,  368,
  369,   -1,  371,  372,  373,  374,  375,  376,  377,   -1,
  379,   -1,  125,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  390,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  401,  402,  403,  404,   -1,   -1,  318,  319,
  320,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  329,
   -1,   -1,  332,  333,  334,  335,  336,  337,  338,  339,
  340,  341,   -1,  343,  344,   -1,  346,  347,  348,  349,
  350,  351,  352,  273,   -1,  355,  356,  357,  358,   -1,
   -1,  361,   -1,   -1,   -1,  365,  366,  367,  368,  369,
   -1,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
   -1,  125,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  390,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  318,  319,
  320,  401,  402,  403,  404,   -1,   -1,   -1,   -1,  329,
   -1,   -1,  332,  333,  334,  335,  336,  337,  338,  339,
  340,  341,   -1,  343,  344,   -1,  346,  347,  348,  349,
  350,  351,  352,   -1,   -1,  355,  356,  357,  358,   -1,
  273,  361,   -1,   -1,   -1,  365,  366,  367,  368,  369,
   -1,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
   -1,  125,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  390,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  401,  402,  403,  404,  318,  319,  320,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,
  333,  334,  335,  336,  337,  338,  339,  340,  341,   -1,
  343,  344,   -1,  346,  347,  348,  349,  350,  351,  352,
   -1,   -1,  355,  356,  357,  358,   -1,   -1,  361,  273,
   -1,   -1,  365,  366,  367,  368,  369,   -1,  371,  372,
  373,  374,  375,  376,  377,   -1,  379,   -1,  125,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  401,  402,
  403,  404,   -1,   -1,  318,  319,  320,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
   -1,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,  125,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,   -1,
   -1,  355,  356,  357,  358,   -1,  273,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,  125,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  401,  402,  403,
  404,  318,  319,  320,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  329,   -1,   -1,  332,  333,  334,  335,  336,
  337,  338,  339,  340,  341,   -1,  343,  344,   -1,  346,
  347,  348,  349,  350,  351,  352,   -1,   -1,  355,  356,
  357,  358,   -1,   -1,  361,  273,   -1,   -1,  365,  366,
  367,  368,  369,   -1,  371,  372,  373,  374,  375,  376,
  377,   -1,  379,   -1,  125,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  390,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  401,  402,  403,  404,   -1,   -1,
  318,  319,  320,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  329,   -1,   -1,  332,  333,  334,  335,  336,  337,
  338,  339,  340,  341,   -1,  343,  344,   -1,  346,  347,
  348,  349,  350,  351,  352,  273,   -1,  355,  356,  357,
  358,   -1,   -1,  361,   -1,   -1,   -1,  365,  366,  367,
  368,  369,   -1,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,   -1,  125,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  390,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  318,  319,  320,  401,  402,  403,  404,   -1,   -1,   -1,
   -1,  329,   -1,   -1,  332,  333,  334,  335,  336,  337,
  338,  339,  340,  341,   -1,  343,  344,   -1,  346,  347,
  348,  349,  350,  351,  352,   -1,   -1,  355,  356,  357,
  358,   -1,  273,  361,   -1,   -1,   -1,  365,  366,  367,
  368,  369,   -1,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,   -1,  125,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  390,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  401,  402,  403,  404,  318,  319,  320,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  329,   -1,
   -1,  332,  333,  334,  335,  336,  337,  338,  339,  340,
  341,   -1,  343,  344,   -1,  346,  347,  348,  349,  350,
  351,  352,   60,   -1,  355,  356,  357,  358,   -1,   -1,
  361,  273,   -1,   -1,  365,  366,  367,  368,  369,   -1,
  371,  372,  373,  374,  375,  376,  377,   -1,  379,   -1,
   -1,   -1,   -1,   91,   -1,   -1,   -1,   -1,   -1,  390,
   -1,   -1,   41,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  401,  402,  403,  404,   -1,   -1,  318,  319,  320,   -1,
   -1,   60,   -1,   -1,   -1,  123,   -1,  329,   -1,   -1,
  332,  333,  334,  335,  336,  337,  338,  339,  340,  341,
   -1,  343,  344,   -1,  346,  347,  348,  349,  350,  351,
  352,  273,   91,  355,  356,  357,  358,   -1,   -1,  361,
   60,   -1,   -1,  365,  366,  367,  368,  369,   -1,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,   -1,   -1,
   -1,   -1,   -1,   -1,  123,   -1,   -1,   -1,  390,   -1,
   -1,   91,   -1,   -1,   -1,   -1,  318,  319,  320,  401,
  402,  403,  404,   -1,   -1,   -1,   60,  329,   -1,   -1,
  332,  333,  334,  335,  336,  337,  338,  339,  340,  341,
   -1,  343,  344,  123,  346,  347,  348,  349,  350,  351,
  352,   60,   -1,  355,  356,  357,  358,   91,   -1,  361,
   -1,   -1,   -1,  365,  366,  367,  368,  369,   -1,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,   60,   -1,
   -1,   -1,   91,   -1,   -1,   -1,  264,   -1,  390,  123,
   -1,   -1,   -1,   60,   -1,  273,   -1,   -1,   -1,  401,
  402,  403,  404,  281,  282,  283,  284,  285,   60,   91,
   -1,   -1,   -1,   -1,  123,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  300,  301,   91,  303,  304,  305,  306,  307,
  308,   -1,   -1,   -1,   -1,   -1,   -1,   60,   -1,   91,
   -1,  123,   -1,   60,   -1,  264,   -1,   -1,   -1,   -1,
   60,   -1,   -1,   -1,  273,   -1,  123,   -1,   -1,  278,
   -1,   -1,  281,  282,  283,  284,  285,   -1,   91,   -1,
   -1,  123,   -1,   -1,   91,   -1,   -1,   -1,   60,   -1,
   -1,   91,   -1,   60,  264,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  273,   -1,   -1,   -1,   -1,   -1,   -1,
  123,  281,  282,  283,  284,  285,  123,   60,  125,   91,
   -1,   -1,   -1,  123,   91,  125,   -1,   -1,   -1,   -1,
   -1,   60,  302,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  264,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   91,  273,
   93,  123,   -1,   -1,   -1,   -1,  123,  281,  282,  283,
  284,  285,   91,   -1,   -1,  264,   60,   -1,   -1,   -1,
   -1,   60,   -1,   -1,  273,  299,   -1,   -1,   -1,   -1,
  123,   -1,  281,  282,  283,  284,  285,   -1,   -1,   -1,
   -1,   -1,  264,  292,  123,   -1,   -1,   91,  297,  298,
   -1,  273,   91,   -1,   -1,   -1,  278,  264,   -1,  281,
  282,  283,  284,  285,   -1,  125,  273,   -1,   -1,   -1,
   -1,  293,  264,   -1,  281,  282,  283,  284,  285,  123,
  125,  273,   -1,   -1,  123,   -1,  293,   -1,   -1,  281,
  282,  283,  284,  285,   -1,   -1,   -1,   -1,   -1,   -1,
  292,  264,  265,   -1,   -1,   -1,   -1,  264,  265,   -1,
  273,  274,   -1,   -1,  264,   -1,  273,  274,  281,  282,
  283,  284,  285,  273,  281,  282,  283,  284,  285,   -1,
   -1,  281,  282,  283,  284,  285,   -1,   -1,   -1,   -1,
   -1,   -1,  264,  265,   -1,   -1,   -1,  264,   -1,   -1,
   -1,  273,  274,  261,  262,   -1,  273,  274,   -1,  281,
  282,  283,  284,  285,  281,  282,  283,  284,  285,   -1,
   -1,  264,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  273,   -1,   -1,   -1,   -1,  264,   -1,   -1,  281,  282,
  283,  284,  285,   -1,  273,   -1,   -1,   -1,   -1,  278,
  260,   -1,  281,  282,  283,  284,  285,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  260,  276,   -1,   -1,   -1,
  264,   -1,   -1,   -1,   -1,  264,   -1,   -1,   -1,  273,
   -1,  276,   -1,   -1,  273,   -1,   -1,  281,  282,  283,
  284,  285,  281,  282,  283,  284,  285,  307,  308,  273,
  274,  311,  312,  313,  314,  315,  316,   -1,   -1,   -1,
   -1,   -1,  307,  308,   -1,   -1,  311,  312,  313,  314,
  315,  316,   -1,   -1,  382,  383,  384,  385,   -1,   -1,
   -1,   -1,   -1,  391,  392,  393,  394,  395,  396,  397,
  398,  399,  400,   -1,  318,  319,  320,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
  274,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
  274,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
  274,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
  274,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
  274,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
  274,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
   -1,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
   -1,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
   -1,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
   -1,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,  273,
   -1,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  318,  319,  320,  401,  402,  403,
  404,   -1,   -1,   -1,   -1,  329,   -1,   -1,  332,  333,
  334,  335,  336,  337,  338,  339,  340,  341,   -1,  343,
  344,   -1,  346,  347,  348,  349,  350,  351,  352,   -1,
   -1,  355,  356,  357,  358,   -1,   -1,  361,   -1,   -1,
   -1,  365,  366,  367,  368,  369,   -1,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  390,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  401,  402,  403,
  404,  329,   -1,   -1,  332,  333,  334,  335,  336,  337,
  338,  339,  340,  341,   -1,  343,  344,   -1,  346,  347,
  348,  349,  350,  351,  352,   -1,   -1,  355,  356,  357,
  358,   -1,   -1,  361,   -1,   -1,   -1,  365,  366,  367,
  368,  369,   -1,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  390,  257,  258,  259,  260,  261,  262,  263,
   -1,  265,   -1,  401,  402,  403,  404,   -1,   -1,   -1,
  274,   -1,  276,   -1,   -1,  279,   -1,   -1,   -1,   -1,
   -1,   -1,  286,
  };

#line 1042 "Repil/IR/IR.jay"

}

#line default
namespace yydebug {
        using System;
	 internal interface yyDebug {
		 void push (int state, Object value);
		 void lex (int state, int token, string name, Object value);
		 void shift (int from, int to, int errorFlag);
		 void pop (int state);
		 void discard (int state, int token, string name, Object value);
		 void reduce (int from, int to, int rule, string text, int len);
		 void shift (int from, int to);
		 void accept (Object value);
		 void error (string message);
		 void reject ();
	 }
	 
	 class yyDebugSimple : yyDebug {
		 void println (string s){
			 System.Diagnostics.Debug.WriteLine (s);
		 }
		 
		 public void push (int state, Object value) {
			 println ("push\tstate "+state+"\tvalue "+value);
		 }
		 
		 public void lex (int state, int token, string name, Object value) {
			 println("lex\tstate "+state+"\treading "+name+"\tvalue "+value);
		 }
		 
		 public void shift (int from, int to, int errorFlag) {
			 switch (errorFlag) {
			 default:				// normally
				 println("shift\tfrom state "+from+" to "+to);
				 break;
			 case 0: case 1: case 2:		// in error recovery
				 println("shift\tfrom state "+from+" to "+to
					     +"\t"+errorFlag+" left to recover");
				 break;
			 case 3:				// normally
				 println("shift\tfrom state "+from+" to "+to+"\ton error");
				 break;
			 }
		 }
		 
		 public void pop (int state) {
			 println("pop\tstate "+state+"\ton error");
		 }
		 
		 public void discard (int state, int token, string name, Object value) {
			 println("discard\tstate "+state+"\ttoken "+name+"\tvalue "+value);
		 }
		 
		 public void reduce (int from, int to, int rule, string text, int len) {
			 println("reduce\tstate "+from+"\tuncover "+to
				     +"\trule ("+rule+") "+text);
		 }
		 
		 public void shift (int from, int to) {
			 println("goto\tfrom state "+from+" to "+to);
		 }
		 
		 public void accept (Object value) {
			 println("accept\tvalue "+value);
		 }
		 
		 public void error (string message) {
			 println("error\t"+message);
		 }
		 
		 public void reject () {
			 println("reject");
		 }
		 
	 }
}
// %token constants
 class Token {
  public const int INTEGER = 257;
  public const int HEX_INTEGER = 258;
  public const int FLOAT_LITERAL = 259;
  public const int STRING = 260;
  public const int TRUE = 261;
  public const int FALSE = 262;
  public const int UNDEF = 263;
  public const int VOID = 264;
  public const int NULL = 265;
  public const int LABEL = 266;
  public const int X = 267;
  public const int SOURCE_FILENAME = 268;
  public const int TARGET = 269;
  public const int DATALAYOUT = 270;
  public const int TRIPLE = 271;
  public const int GLOBAL_SYMBOL = 272;
  public const int LOCAL_SYMBOL = 273;
  public const int META_SYMBOL = 274;
  public const int META_SYMBOL_DEF = 275;
  public const int SYMBOL = 276;
  public const int DISTINCT = 277;
  public const int METADATA = 278;
  public const int CONSTANT_BYTES = 279;
  public const int TYPE = 280;
  public const int HALF = 281;
  public const int FLOAT = 282;
  public const int DOUBLE = 283;
  public const int X86_FP80 = 284;
  public const int INTEGER_TYPE = 285;
  public const int ZEROINITIALIZER = 286;
  public const int OPAQUE = 287;
  public const int DEFINE = 288;
  public const int DECLARE = 289;
  public const int UNNAMED_ADDR = 290;
  public const int LOCAL_UNNAMED_ADDR = 291;
  public const int NOALIAS = 292;
  public const int ELLIPSIS = 293;
  public const int GLOBAL = 294;
  public const int CONSTANT = 295;
  public const int PRIVATE = 296;
  public const int INTERNAL = 297;
  public const int EXTERNAL = 298;
  public const int FASTCC = 299;
  public const int SIGNEXT = 300;
  public const int ZEROEXT = 301;
  public const int VOLATILE = 302;
  public const int RETURNED = 303;
  public const int NONNULL = 304;
  public const int NOCAPTURE = 305;
  public const int WRITEONLY = 306;
  public const int READONLY = 307;
  public const int READNONE = 308;
  public const int ATTRIBUTE_GROUP_REF = 309;
  public const int ATTRIBUTES = 310;
  public const int NORECURSE = 311;
  public const int NOUNWIND = 312;
  public const int SPECULATABLE = 313;
  public const int SSP = 314;
  public const int UWTABLE = 315;
  public const int ARGMEMONLY = 316;
  public const int SEQ_CST = 317;
  public const int RET = 318;
  public const int BR = 319;
  public const int SWITCH = 320;
  public const int INDIRECTBR = 321;
  public const int INVOKE = 322;
  public const int RESUME = 323;
  public const int CATCHSWITCH = 324;
  public const int CATCHRET = 325;
  public const int CLEANUPRET = 326;
  public const int UNREACHABLE = 327;
  public const int FNEG = 328;
  public const int ADD = 329;
  public const int NUW = 330;
  public const int NSW = 331;
  public const int FADD = 332;
  public const int SUB = 333;
  public const int FSUB = 334;
  public const int MUL = 335;
  public const int FMUL = 336;
  public const int UDIV = 337;
  public const int SDIV = 338;
  public const int FDIV = 339;
  public const int UREM = 340;
  public const int SREM = 341;
  public const int FREM = 342;
  public const int SHL = 343;
  public const int LSHR = 344;
  public const int EXACT = 345;
  public const int ASHR = 346;
  public const int AND = 347;
  public const int OR = 348;
  public const int XOR = 349;
  public const int EXTRACTELEMENT = 350;
  public const int INSERTELEMENT = 351;
  public const int SHUFFLEVECTOR = 352;
  public const int EXTRACTVALUE = 353;
  public const int INSERTVALUE = 354;
  public const int ALLOCA = 355;
  public const int LOAD = 356;
  public const int STORE = 357;
  public const int FENCE = 358;
  public const int CMPXCHG = 359;
  public const int ATOMICRMW = 360;
  public const int GETELEMENTPTR = 361;
  public const int ALIGN = 362;
  public const int INBOUNDS = 363;
  public const int INRANGE = 364;
  public const int TRUNC = 365;
  public const int ZEXT = 366;
  public const int SEXT = 367;
  public const int FPTRUNC = 368;
  public const int FPEXT = 369;
  public const int TO = 370;
  public const int FPTOUI = 371;
  public const int FPTOSI = 372;
  public const int UITOFP = 373;
  public const int SITOFP = 374;
  public const int PTRTOINT = 375;
  public const int INTTOPTR = 376;
  public const int BITCAST = 377;
  public const int ADDRSPACECAST = 378;
  public const int ICMP = 379;
  public const int EQ = 380;
  public const int NE = 381;
  public const int UGT = 382;
  public const int UGE = 383;
  public const int ULT = 384;
  public const int ULE = 385;
  public const int SGT = 386;
  public const int SGE = 387;
  public const int SLT = 388;
  public const int SLE = 389;
  public const int FCMP = 390;
  public const int OEQ = 391;
  public const int OGT = 392;
  public const int OGE = 393;
  public const int OLT = 394;
  public const int OLE = 395;
  public const int ONE = 396;
  public const int ORD = 397;
  public const int UEQ = 398;
  public const int UNE = 399;
  public const int UNO = 400;
  public const int PHI = 401;
  public const int SELECT = 402;
  public const int CALL = 403;
  public const int TAIL = 404;
  public const int VA_ARG = 405;
  public const int LANDINGPAD = 406;
  public const int CATCHPAD = 407;
  public const int CLEANUPPAD = 408;
  public const int yyErrorCode = 256;
 }
 namespace yyParser {
  using System;
  /** thrown for irrecoverable syntax errors and stack overflow.
    */
  internal class yyException : System.Exception {
    public yyException (string message) : base (message) {
    }
  }
  internal class yyUnexpectedEof : yyException {
    public yyUnexpectedEof (string message) : base (message) {
    }
    public yyUnexpectedEof () : base ("") {
    }
  }

  /** must be implemented by a scanner object to supply input to the parser.
    */
  internal interface yyInput {
    /** move on to next token.
        @return false if positioned beyond tokens.
        @throws IOException on input error.
      */
    bool advance (); // throws java.io.IOException;
    /** classifies current token.
        Should not be called if advance() returned false.
        @return current %token or single character.
      */
    int token ();
    /** associated with current token.
        Should not be called if advance() returned false.
        @return value for token().
      */
    Object value ();
  }
 }
} // close outermost namespace, that MUST HAVE BEEN opened in the prolog
