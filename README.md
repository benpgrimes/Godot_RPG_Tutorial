This project follows along to the youtube tutorial created by github user uheartbeast. 

His profile can be found here: https://github.com/uheartbeast

The original Repo for this project can be found here: https://github.com/uheartbeast/youtube-tutorials/tree/master/Action%20RPG

The youtube tutorial playlist can be found here: https://www.youtube.com/watch?v=mAbG8Oi-SvQ&list=PL9FzW-m48fn2SlrW0KoLT4n5egNdX-W9a&index=1


This project uses Godot 4, while the original uses Godot 3.2.

This project uses C#, while the original uses Godots native language GDScript.


There are a few things that I would change if I were to continue this project, 
but since this was just following a tutorial series I decided to leave the project as is
so as not to mess with commit history too much.
Some of these changes would include but are not limited to:

Reorganizing the file structure. Splitting it out into "Scenes" and "Scripts" was silly, 
and after adding a substantial amout of both it makes it harder to traverse.
The file structure he has in his videos are far better.

Fixing a few capitilization discrepencies. Hitbox and Hurtbox scene files are lowercase for example,
however git add doesnt automatically detect capitilization differences. 
I know there is a work around for this; I believe changing the name, adding it,
then changing it back with the capitilization difference and adding again will fix it,
but I can't be bothered. 
There are a few other discrepencies within the code too.

Reordering methods based on call order.
