# SpeedTester
SpeedTester running with Mono (e.g. for running on a Synology NAS)

[![Build Status](https://travis-ci.org/dhcgn/SpeedTester.svg?branch=master)](https://travis-ci.org/dhcgn/SpeedTester)
[![Code Climate](https://codeclimate.com/github/dhcgn/SpeedTester/badges/gpa.svg)](https://codeclimate.com/github/dhcgn/SpeedTester)
[![Test Coverage](https://codeclimate.com/github/dhcgn/SpeedTester/badges/coverage.svg)](https://codeclimate.com/github/dhcgn/SpeedTester/coverage)
[![Issue Count](https://codeclimate.com/github/dhcgn/SpeedTester/badges/issue_count.svg)](https://codeclimate.com/github/dhcgn/SpeedTester)


## Motivation
I want to measure my internet connection speed and receive an email with the result.

## How To
### Windows

Just use the Windows Task Scheduler.

### Linux

See Synology NAS, should be the same with the exception of Tasks.

### Synology NAS

1. Install Mono

   You need to install mono to run .net executables. You can use the Synology or the community package.
   ![Install Mono](http://i.imgur.com/UuD9xLt.png "Install Mono")
2. Place executable

   ![Place executable](http://i.imgur.com/HOAJfvI.png "Place executable")

3. Edit Config

   See `config.sample.xml` and copy it to `config.xml`. You only need to change the email settings.
   If you want to can add or remove some download urls.

4. Add Task

   ![Add Task](http://i.imgur.com/dAfhYgE.png "Add Task")

## Sample Report

   ![Sample Report](http://i.imgur.com/7rF5Q6d.png "Sample Report")
