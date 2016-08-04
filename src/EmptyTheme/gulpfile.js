/// <binding BeforeBuild='cleanup, copy' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var clean = require('gulp-clean');

gulp.task('cleanup', function () {
    gulp.src('./wwwroot/css', { read: false }).pipe(clean());
    gulp.src('./wwwroot/fonts', { read: false }).pipe(clean());
    gulp.src('./wwwroot/js', { read: false }).pipe(clean());
});

gulp.task('copy', function () {
    gulp.src('./node_modules/bootstrap/dist/css/*.min.css').pipe(gulp.dest('./wwwroot/css'));
    gulp.src('./node_modules/bootstrap/dist/fonts/*').pipe(gulp.dest('./wwwroot/fonts'));
    gulp.src('./node_modules/bootstrap/dist/js/*.min.js').pipe(gulp.dest('./wwwroot/js'));
});
