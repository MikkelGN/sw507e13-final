#Session -1600301272

set output "rightstep.eps"
set term postscript
set title "Acceleration Graph"
set xlabel "Time (ms)"
set ylabel "Acceleration (g)"
set autoscale
plot "rightstepdata.dat" title "Acceleration" with lines lc rgb 'red'
