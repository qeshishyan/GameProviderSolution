import React, { useEffect, useState } from 'react'

import { useGetPostsQuery } from '../api/posts/jsonPlaceholderApi'
import { ApplicationBackground } from '../components/ApplicationBackground/ApplicationBackground'
import { BetRoofTiles } from '../components/BetRoofTiles/BetRoofTiles'
import GameContainer from '../components/GameContainer/GameContainer'
import { GameSettings } from '../components/GameSettings/GameSettings'
import LastBets from '../components/LastBets/LastBets'
import TopBets from '../components/TopCoefficients/TopCoefficients'

const Game = () => {
  const [backgroundMovingTime, setBackgroundMovingTime] = useState(0)
  const { data: components = [] } = useGetPostsQuery(null)
  console.group('Game')
  console.log('components: ', components)
  console.groupEnd()

  useEffect(() => {
    const root = document.documentElement
    const bgHeight = 6808
    const bgWidth = 1915
    const heightProportioned = (root?.offsetWidth * bgHeight) / bgWidth
    const containerHeight = root?.clientHeight !== 0 ? root?.clientHeight : 0
    root?.style.setProperty(
      '--slidedown-bg-height',
      `${heightProportioned + containerHeight}px`
    )
  }, [])

  return (
    <>
      <ApplicationBackground backgroundMovingTime={backgroundMovingTime}>
        <div className="game-wrapper">
          <div className="game-roof-row">
            <div className="game-roof-tiles">
              <BetRoofTiles />
            </div>
            <div className="game-settings">
              <GameSettings />
            </div>
          </div>

          <div className="game-middle-row">
            <div className="game-left-column">
              <TopBets />
              <LastBets />
            </div>
            <div className="game-playable-container-wrapper">
              <GameContainer
                handleBackgroundAnimation={(backgroundMovingTime: number) => {
                  setBackgroundMovingTime(backgroundMovingTime || 0)
                }}
              />
            </div>
            <div className="game-right-column"></div>
          </div>
        </div>
      </ApplicationBackground>
    </>
  )
}

export default Game
