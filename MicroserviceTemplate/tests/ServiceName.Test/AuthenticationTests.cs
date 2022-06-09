using Antlr4.Runtime.Misc;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using ServiceName.Infrastructure.Authentication;
using SQLParser.Enums;
using SQLParser.Parsers.TSql;

namespace ServiceName.Test
{
    public class AuthenticationTests
    {
        [Fact]
        public void GenerateAndValidateJwtToken()
        {
            MockJwtAuthenticationService authService = new MockJwtAuthenticationService();
            var token = authService.GenerateToken(1);
            var isValid = authService.ValidateToken(token);
        }

        [Fact]
        public void TestSQLParser()
        {
            //https://devblogs.microsoft.com/azure-sql/programmatically-parsing-transact-sql-t-sql-with-the-scriptdom-parser/
            using (var rdr = new StringReader("SELECT Settings FROM ServiceName_Setting WHERE TenantId=1"))
            {
                IList<ParseError> errors = null;
                var parser = new TSql150Parser(true, SqlEngineType.All);
                var tree = parser.Parse(rdr, out errors);
                MyVisitor checker = new MyVisitor();
                tree.Accept(checker);
            }
        }
    }

    class MyVisitor : TSqlFragmentVisitor
    {
        internal bool containsOnlySelects = true;
        public readonly List<SelectStatement> SelectStatements = new List<SelectStatement>();

        public override void Visit(SelectStatement node)
        {
            SelectStatements.Add(node);

            base.Visit(node);
        }
    }
}