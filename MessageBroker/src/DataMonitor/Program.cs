using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataMonitor
{
    class Program
    {
        static void Main(string[] args)
        {

            string oradb = "TNS_ADMIN=C:\\Users\\monacol\\Oracle\\network\\admin;USER ID=LEMONACO;PASSWORD=Allianz2021;DATA SOURCE=midasddv.maau.group:1521/midasddv;PERSIST SECURITY INFO=True";

            OracleConnection conn = new OracleConnection(oradb); // C#

            conn.Open();

            OracleCommand cmdRecords = new OracleCommand
            {
                Connection = conn,
                CommandText = "PKG_INCIDENT_REPORT.ListRecordsForProcessing",
                CommandType = CommandType.StoredProcedure
            };

            cmdRecords.Parameters.Add("p_incident_list", OracleDbType.RefCursor, ParameterDirection.InputOutput);
            OracleDataAdapter da = new OracleDataAdapter(cmdRecords);
            DataSet ds = new DataSet();
            da.Fill(ds);
            OracleCommand cmd = new OracleCommand
            {
                Connection = conn,
                CommandText = "PKG_INCIDENT_REPORT.GetFullIncidentData",
                CommandType = CommandType.StoredProcedure
            };

            //PKG_INCIDENT_REPORT.GetFullIncidentData(835, v_data_xml);
            cmd.Parameters.Add("p_incident_journal_id", OracleDbType.Int32, 9, ParameterDirection.Input);

            string xml=string.Empty;
            cmd.Parameters.Add("p_incident_data", OracleDbType.XmlType, xml, ParameterDirection.Output);
            var result = cmd.ExecuteNonQuery();

            xml = ((OracleXmlType)cmd.Parameters["p_incident_data"].Value).Value;

            conn.Dispose();

            var mySerializer = new XmlSerializer(typeof(MidasIncident));
            var myFileStream = new FileStream("C:\\GitHub\\NServiceBus\\src\\DataMonitor\\Files\\Test.xml", FileMode.Open);
            var myObject = (MidasIncident)mySerializer.Deserialize(myFileStream);
        }
    }
}
