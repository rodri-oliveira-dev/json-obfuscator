' Header.cls
Public HeaderName As String
Public HeaderValue As String


' No módulo ou em um arquivo de enumeração separado
Public Enum HttpVerb
    HTTP_GET = 0
    HTTP_POST = 1
    HTTP_PUT = 2
    HTTP_DELETE = 3
End Enum


' HttpRequest function
Public Function HttpRequest(ByVal url As String, _
                            ByVal body As String, _
                            ByVal headers() As Header, _
                            ByVal verb As HttpVerb) As String
    On Error GoTo ErrorHandler
    
    ' Create the XMLHTTP object
    Dim http As Object
    Set http = CreateObject("MSXML2.XMLHTTP.6.0")

    ' Define the verb as a string
    Dim method As String
    Select Case verb
        Case HTTP_GET
            method = "GET"
        Case HTTP_POST
            method = "POST"
        Case HTTP_PUT
            method = "PUT"
        Case HTTP_DELETE
            method = "DELETE"
        Case Else
            Err.Raise vbObjectError + 513, , "Método HTTP inválido."
    End Select

    ' Open the HTTP request
    http.Open method, url, False
    
    ' Add headers
    Dim i As Integer
    For i = LBound(headers) To UBound(headers)
        http.setRequestHeader headers(i).HeaderName, headers(i).HeaderValue
    Next i

    ' Send the request
    If method = "POST" Or method = "PUT" Then
        http.Send body
    Else
        http.Send
    End If

    ' Check for success
    If http.Status >= 200 And http.Status < 300 Then
        HttpRequest = http.responseText
    Else
        Err.Raise vbObjectError + 514, , "Erro na requisição: " & http.Status & " " & http.statusText
    End If

    Exit Function

ErrorHandler:
    HttpRequest = "Erro: " & Err.Description
End Function
