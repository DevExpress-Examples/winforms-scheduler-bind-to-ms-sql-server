Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Data.SqlClient
Imports System.Data.OracleClient

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Const CarsDBConnection As String = "Data Source=(local); Initial Catalog=CarsDB; Integrated Security=SSPI"
		Private CarsDataset As DataSet

		Private CarsAdapter As SqlDataAdapter
		Private CarsConn As SqlConnection

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			CarsDataset = New DataSet()
			Dim selectAppointments As String = "SELECT * FROM CarScheduling"

			CarsConn = New SqlConnection(CarsDBConnection)
			CarsConn.Open()
			CarsAdapter = New SqlDataAdapter(selectAppointments, CarsConn)
			AddHandler CarsAdapter.RowUpdated, AddressOf CarsAdapter_RowUpdated
			CarsAdapter.Fill(CarsDataset, "CarScheduling")

			Me.appointmentsBS.DataSource = CarsDataset
			Me.appointmentsBS.DataMember = "CarScheduling"
			Me.appointmentsBS.Position = 0


			Me.schedulerStorage1.Appointments.DataSource = Me.appointmentsBS
			Me.schedulerStorage1.Appointments.Mappings.AllDay = "AllDay"
			Me.schedulerStorage1.Appointments.Mappings.Description = "Description"
			Me.schedulerStorage1.Appointments.Mappings.End = "EndTime"
			Me.schedulerStorage1.Appointments.Mappings.Label = "Label"
			Me.schedulerStorage1.Appointments.Mappings.Location = "Location"
			Me.schedulerStorage1.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo"
			Me.schedulerStorage1.Appointments.Mappings.ReminderInfo = "ReminderInfo"
			Me.schedulerStorage1.Appointments.Mappings.Start = "StartTime"
			Me.schedulerStorage1.Appointments.Mappings.Status = "Status"
			Me.schedulerStorage1.Appointments.Mappings.Subject = "Subject"
			Me.schedulerStorage1.Appointments.Mappings.Type = "EventType"

			Dim cmdBuilder As New SqlCommandBuilder(CarsAdapter)
			CarsAdapter.InsertCommand = cmdBuilder.GetInsertCommand()
			CarsAdapter.DeleteCommand = cmdBuilder.GetDeleteCommand()
			CarsAdapter.UpdateCommand = cmdBuilder.GetUpdateCommand()
			CarsConn.Close()
		End Sub

		Private Sub CarsAdapter_RowUpdated(ByVal sender As Object, ByVal e As SqlRowUpdatedEventArgs)
			If e.Status = UpdateStatus.Continue AndAlso e.StatementType = StatementType.Insert Then
				Dim id As Integer = 0
				Using cmd As New SqlCommand("SELECT IDENT_CURRENT('CarScheduling')", CarsConn)
					id = Convert.ToInt32(cmd.ExecuteScalar())
				End Using
				e.Row("ID") = id
			End If
		End Sub

		Private Sub OnApptChangedInsertedDeleted(ByVal sender As Object, ByVal e As DevExpress.XtraScheduler.PersistentObjectsEventArgs) Handles schedulerStorage1.AppointmentsChanged, schedulerStorage1.AppointmentsInserted, schedulerStorage1.AppointmentsDeleted
			CarsAdapter.Update(CarsDataset.Tables("CarScheduling"))
		End Sub

	End Class
End Namespace