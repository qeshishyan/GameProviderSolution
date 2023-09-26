import React, { type FC, useEffect, useState } from 'react'

import plusBetDisabled from '../../assets/images/playable-panel/plus-bet-disabled.png'
import plusBet from '../../assets/images/playable-panel/plus-bet.png'
import startButton from '../../assets/images/playable-panel/start-button.png'
import stopButton from '../../assets/images/playable-panel/stop-button.png'
import { HoverComponent } from '../../styles/HoverComponent'
import BetPanel from '../BetPanel/BetPanel'
import classes from './PlayablePanel.module.scss'

export interface PlayablePanelProps {
  handleStartClick?: () => void
  isRoundStarted?: boolean
}

const PlayablePanel: FC<PlayablePanelProps> = ({
  handleStartClick = () => {},
  isRoundStarted = false
}) => {
  const [isStartClicked, setIsStartClicked] = useState(false)
  const [isSingleBetMode, setIsSingleBetMode] = useState(true)
  const autoBets = [100, 200, 300, 400, 1000]
  const [autoBetLeft, setAutoBetLeft] = useState(0)
  const [autoBetRight, setAutoBetRight] = useState(0)

  useEffect(() => {
    if (isRoundStarted) {
      return
    }
    setIsStartClicked(false)
  }, [isRoundStarted])

  const onStartClick = () => {
    if (isStartClicked) {
      return
    }
    setIsStartClicked(true)
    handleStartClick()
  }

  const onPlusBetClick = () => {
    if (isStartClicked) {
      return
    }
    setIsSingleBetMode(false)
  }

  const onAutoBetLeftTileClick = (autoBet: number) => {
    if (isStartClicked) {
      return
    }
    if (autoBet === autoBetLeft) {
      setAutoBetLeft(0)
      return
    }
    setAutoBetLeft(autoBet)
  }

  const onAutoBetRightTileClick = (autoBet: number) => {
    if (isStartClicked) {
      return
    }
    if (autoBet === autoBetRight) {
      setAutoBetRight(0)
      return
    }
    setAutoBetRight(autoBet)
  }

  const getBetTiles = (selectedAutoBet: number, isRight: boolean) => {
    return autoBets.map((autoBet, index) => (
      <HoverComponent
        key={index}
        autoBet={String(autoBet)}
        className={
          (isStartClicked ? classes.autoBetTileDisabled : '') +
          ' ' +
          classes.autoBetTile +
          ' ' +
          (selectedAutoBet === autoBet ? classes.selected : '')
        }
        onClick={() => {
          isRight
            ? onAutoBetRightTileClick(autoBet)
            : onAutoBetLeftTileClick(autoBet)
        }}>
        <span>{autoBet}</span>
      </HoverComponent>
    ))
  }

  return (
    <>
      {isSingleBetMode ? (
        <div
          className={
            classes.playablePanelContainer +
            ' ' +
            (isStartClicked ? classes.buttonDisabled : '')
          }>
          <img
            src={isStartClicked ? stopButton : startButton}
            onClick={() => {
              onStartClick()
            }}
            style={{ width: '15%' }}
            alt=""
          />
          <div className={classes.betPanel}>
            <div className={classes.autoBetTiles}>
              {getBetTiles(autoBetLeft, false)}
            </div>
            <div className={classes.betPanelContainer}>
              <BetPanel
                autoBetValue={autoBetLeft}
                isSingleBetMode={isSingleBetMode}
              />
            </div>
          </div>
          <img
            src={isStartClicked ? plusBetDisabled : plusBet}
            style={{ width: '15%', padding: '4%' }}
            onClick={() => {
              onPlusBetClick()
            }}
            alt=""
          />
        </div>
      ) : (
        <div
          className={
            classes.playablePanelContainer +
            ' ' +
            (isStartClicked ? classes.buttonDisabled : '')
          }>
          <img
            src={isStartClicked ? stopButton : startButton}
            onClick={() => {
              onStartClick()
            }}
            style={{ width: '15%' }}
            alt=""
          />
          <div className={classes.betPanel}>
            <div className={classes.autoBetTilesNonSingleMode}>
              <div className={classes.autoBetTiles}>
                {getBetTiles(autoBetLeft, false)}
              </div>
              <div className={classes.autoBetTiles}>
                {getBetTiles(autoBetRight, true)}
              </div>
            </div>
            <div
              className={
                classes.betPanelContainer +
                ' ' +
                (isSingleBetMode ? '' : classes.nonSingleMode)
              }>
              <BetPanel
                autoBetValue={autoBetLeft}
                isSingleBetMode={isSingleBetMode}
              />
              <BetPanel
                autoBetValue={autoBetRight}
                isSingleBetMode={isSingleBetMode}
              />
            </div>
          </div>
          <img
            src={isStartClicked ? stopButton : startButton}
            onClick={() => {
              onStartClick()
            }}
            style={{ width: '15%' }}
            alt=""
          />
        </div>
      )}
    </>
  )
}

export default PlayablePanel
