#Session -862085142


set output "rotation.eps"
set term postscript
set title "Acceleration Graph"
set xlabel "Time (ms)"
set ylabel "Acceleration (g)"
set autoscale
plot "rotationdata.dat" using 1:2 title "x-axis" with lines lc rgb 'red', "rotationdata.dat" using 1:3 title "y-axis" with lines lc rgb 'blue', "rotationdata.dat" using 1:4 title "z-axis" with lines lc rgb 'black'