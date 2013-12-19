#Session 708135743

set output "leftstep.eps"
set term postscript
set title "Acceleration Graph"
set xlabel "Time (ms)"
set ylabel "Acceleration (g)"
set autoscale
plot "leftstepdata.dat" title "Acceleration" with lines lc rgb 'red'
