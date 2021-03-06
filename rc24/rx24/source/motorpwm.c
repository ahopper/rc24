/*
Copyright 2008 - 2009 � Alan Hopper

	This file is part of rc24.

    rc24 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    rc24 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with rc24.  If not, see <http://www.gnu.org/licenses/>.


*/


 #include <jendefs.h>
#include <AppHardwareApi.h>

#include "motorpwm.h"
#include "config.h"

int pwmperiod=20*16; //50KHz
//todo make frequency selectable and support timer 2 on JN5148
void initMotorPwm(uint32 pin)
{
    uint8 timer;
    if(pin==E_AHI_DIO10_INT)timer=E_AHI_TIMER_0;
    else if(pin==E_AHI_DIO13_INT)timer=E_AHI_TIMER_1;
    else return;
/* set up timer  PWM */
    vAHI_TimerEnable(timer,
                     0,
                     FALSE,
                     FALSE,
                     TRUE);

    vAHI_TimerClockSelect(timer,
                          FALSE,
                          TRUE);

    vAHI_TimerStartRepeat(timer,
                          pwmperiod,       // low period (space)
                          pwmperiod);      // period

}
void setMotorPWM(int channel, uint16 ontime)
{
	//ontime 0-4096

	uint32 on=ontime*pwmperiod/4096;
#ifdef JN5148
    uint8 timer;
    if(channel==E_AHI_DIO10_INT)timer=E_AHI_TIMER_0;
    else if(channel==E_AHI_DIO13_INT)timer=E_AHI_TIMER_1;
    else return;

    vAHI_TimerStartRepeat(timer,
                            pwmperiod-(uint16)on,   // low period (space)
                            pwmperiod);      // period

#else
    if(channel==E_AHI_DIO10_INT)WRITE_REG32(0x50000004,pwmperiod-(uint16)on);
    else WRITE_REG32(0x60000004,pwmperiod-(uint16)on);
#endif
}
