<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128633804/12.2.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E551)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [DXDBScheduler.sql](./CS/SchedulerSQLRuntime/DXDBScheduler.sql) (VB: [DXDBScheduler.sql](./VB/SchedulerSQLRuntime/DXDBScheduler.sql))
* [Form1.cs](./CS/SchedulerSQLRuntime/Form1.cs) (VB: [Form1.vb](./VB/SchedulerSQLRuntime/Form1.vb))
<!-- default file list end -->
# How to bind SchedulerControl to MS SQL Server database at runtime


<p>This example shows how you can bind the SchedulerControl to an <strong>SQL Server</strong> database at runtime. The <strong>SqlCommandBuilder</strong> is used to generate SQL queries, however you can modify them as required or specify your own queries.<br />
The project uses MS SQL Server database. You can create a new database using the .sql script file included in the project, or use already existing database. Check the code that specifies mappings to ensure that all data fields are correctly mapped to appointment properties. <br />
Note that mappings for the Start and End appointment properties are required.  The UniqueID field in the Appointments table is not mapped, however, because it is an identity auto-incremented field updated by MS SQL Sever itself. If you map it for whatever reason, make sure that the <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerAppointmentStorage_CommitIdToDataSourcetopic"><u>CommitIdToDataSource</u></a> property is set to <strong>false</strong>.<br />
This project sets the <a href="http://documentation.devexpress.com/#WPF/DevExpressXpfSchedulerAppointmentStorage_ResourceSharingtopic"><u>ResourceSharing</u></a> property to <strong>true</strong>, so each appointment can be assigned to several resources. The corresponding resource IDs arte stored in XML format in the ResourceIDs field. <br />
If the <strong>ResourceSharing </strong>property is false (by default), then the <a href="http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraSchedulerAppointmentMappingInfo_ResourceIdtopic"><u>AppointmentMappingInfo.ResourceId</u></a> property should be set to the database field containing the value of the resource ID with which an appointment is associated. Therefore, this field must have the same type as the resource ID. The Scheduler does not restrict the type of resource ID to a particular .NET type, so you can use any data type if the types of the corresponding fields in Appointment and Resource tables will match.</p><p>If your database server is not MS SQL, you can replace SqlDataAdapter and SqlCommandBuilder with the corresponding data adapter and command builder, such as <i>OracleDataAdapter</i> and <i>OracleCommandBuilder</i>.</p>


<h3>Description</h3>

<p>The sample for v2012 vol 2.4 and higher uses MS SQL Server database file included in the project.  The database script is attached as the DXDBScheduler.sql file. Instead of System.Windows.Forms.BindingSource instances the SchedulerStorage is bound to the System.Data.Dataset object which provides data for appointments and resources. The dataset is filled by System.Data.SqlClient.SqlDataAdapter instances at runtime.<br />
</p>

<br/>


