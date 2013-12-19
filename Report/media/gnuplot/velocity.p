#Session- 1839111524

set output "velocity.eps"
set term postscript
set title "Velocity Graph"
set xlabel "Time (ms)"
set ylabel "Velocity (m/s)"
set autoscale
plot "Velocity.dat" title "Velocity" with lines lc rgb 'blue'
