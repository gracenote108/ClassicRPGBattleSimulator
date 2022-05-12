# Classic RPG Battle Simulator

This project is meant to replicate the classic 90s RPGs battle system. In this system, characters are lined up in a row with a menu frame on the bottom where status and other data about the character are listed.

Each game had their own spin on it, but almost all of them had a sort of Active Time Battle Bar (ATB Bar). This bar would fill up to the max which would indicate the character is ready for an action.

The display for my program is simple but it behaves in such a manner as described above.

In this little program I use various technologies to get my work done:
1. Multithreading and asynchronous systems are used to drive the handling of each characters separately in their own space rather than one at a time.
2. Event handling is used to drive the entire battle forward.
3. Used the Producer/Consumer design pattern to manage the Linked List Queue I designed to execute a characters selected action.
4. All windows controls were created programmatically rather than XAML.