import React, { type FC } from 'react'

import settingCircle from '../../assets/images/settings-circle.png'
import classes from './GameSettings.module.scss'

export const GameSettings: FC = () => {
  return (
    <div className={classes.settingsContainer}>
      <img src={settingCircle} alt="Setting 1" />
      <img src={settingCircle} alt="Setting 2" />
      <img src={settingCircle} alt="Setting 3" />
    </div>
  )
}
