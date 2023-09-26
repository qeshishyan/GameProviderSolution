import React, { type FC, useState } from 'react'

import autoBetTile from '../../assets/images/playable-panel/auto-bet-tile.png'
import classes from './BetPanel.module.scss'

export interface BetPanelProps {
  autoBetValue?: number
  isSingleBetMode?: boolean
}

const BetPanel: FC<BetPanelProps> = ({
  autoBetValue = 0,
  isSingleBetMode = true
}) => {
  // betBar takes 90% of width => 5% makes us start where the bar starts
  const selectedAutoBetTileLeftMargin =
    5 + (autoBetValue === 0 ? 0 : (autoBetValue / 10 - 10) * 0.9)
  const betBarValueLeftMargin = autoBetValue === 0 ? 0 : autoBetValue / 10 - 10
  const [isAutoBetChecked, setIsAutoBetChecked] = useState(false)
  const [isAutoCashOutChecked, setIsAutoCashOutChecked] = useState(false)

  const onAutoBetChecked = () => {
    if (isAutoBetChecked) {
      setIsAutoBetChecked(false)
      return
    }
    setIsAutoBetChecked(true)
  }

  const onAutoCashOutChecked = () => {
    if (isAutoCashOutChecked) {
      setIsAutoCashOutChecked(false)
      return
    }
    setIsAutoCashOutChecked(true)
  }

  return (
    <div
      className={
        classes.betPanelContainer +
        ' ' +
        (isSingleBetMode ? '' : classes.nonSingleBetMode)
      }>
      <div
        className={classes.selectedAutoBetTile}
        style={{
          opacity: autoBetValue === 0 ? 0 : 1,
          marginLeft: selectedAutoBetTileLeftMargin + '%'
        }}>
        <span>{autoBetValue}</span>
        <img src={autoBetTile} alt="" />
      </div>
      <div className={classes.betBarContainer}>
        <span>min</span>
        <div className={classes.betBar}>
          {autoBetValue === 0 ? null : (
            <div
              className={classes.betBarValue}
              style={{ marginLeft: betBarValueLeftMargin + '%' }}
            />
          )}
        </div>
        <span>max</span>
      </div>

      <div
        className={
          isSingleBetMode
            ? classes.betCheckBoxesSingleMode
            : classes.betCheckBoxesNonSingleMode
        }>
        <div className={classes.betCheckBox}>
          <span>auto-bet</span>
          <div className={classes.autoBetCheckBoxImg}>
            <span>{autoBetValue === 0 ? 100 : autoBetValue}</span>
            <img src={autoBetTile} alt="" />
          </div>
          <label
            className={classes.autoBetToggle}
            style={{
              backgroundColor: isAutoBetChecked ? '#08d3ff' : '#003564'
            }}>
            <input type="checkbox" onChange={onAutoBetChecked} />
            <span
              className={classes.slider}
              style={{
                backgroundColor: isAutoBetChecked ? '#08d3ff' : '#003564'
              }}
            />
            <span className={classes.labels} data-on="ON" data-off="OFF" />
          </label>
        </div>

        {isSingleBetMode ? <div className={classes.verticalLine} /> : null}

        <div className={classes.betCheckBox}>
          <span>auto-cashout</span>
          <div className={classes.autoBetCheckBoxImg}>
            <span>2.5</span>
            <img src={autoBetTile} alt="" />
          </div>
          <label
            className={classes.autoBetToggle}
            style={{
              backgroundColor: isAutoCashOutChecked ? '#08d3ff' : '#003564'
            }}>
            <input type="checkbox" onChange={onAutoCashOutChecked} />
            <span
              className={classes.slider}
              style={{
                backgroundColor: isAutoCashOutChecked ? '#08d3ff' : '#003564'
              }}
            />
            <span className={classes.labels} data-on="ON" data-off="OFF" />
          </label>
        </div>
      </div>
    </div>
  )
}

export default BetPanel
