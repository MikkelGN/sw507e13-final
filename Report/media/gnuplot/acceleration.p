#Session -1839111524

set output "acceleration.eps"
set term postscript
set title "Acceleration Graph"
set xlabel "Time (ms)"
set ylabel "Acceleration (g)"
set autoscale
plot "acceleration.dat" title "Acceleration" with lines lc rgb 'red'
