<html>
    
    <head>
        <script src="Scripts/jquery-1.6.4.js"></script>
    </head>

   <body>

    Output: <pre id="output">Loading...</pre>
       
     <script type="text/javascript">
         $(function () {
             $.get("API/Mail/List", function(data) {
                 $("#output").html(JSON.stringify(data));
             });
        });
    </script>


   </body>

</html>