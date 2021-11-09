# MIGRACIONES BBDD
- Add-Migration 'nombre'
- Update-database

# PUBLICAR APP BACKEND EN EL SERVIDOR BARATARIA

- Publicamos el proyecto en una carpeta llamada "BackEnd" y se generarán una serie de ficheros.
- Nos conectamos al servidor barataria: 10.60.3.58 
- Copiamos la carpeta "BackEnd" en la ruta: /opt/barataria (Si existe una carpeta con este nombre la eliminamos).
- Paramos y reiniciamos el servidor:
- service barataria stop
- service barataria start

